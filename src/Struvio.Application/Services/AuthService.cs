using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Struvio.Common.Models.AuthModels;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Struvio.Application.Services;

/// <summary>
/// Authentication servisinin implementasyonu.
/// Kullanıcı girişi, JWT token oluşturma ve oturum yönetimi işlemlerini gerçekleştirir.
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IStruvioLogger _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext,
        IConfiguration configuration,
        IStruvioLogger logger)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Kullanıcı girişi yapar, JWT token oluşturur ve UserSession kaydı ekler.
    /// </summary>
    public async Task<LoginResponseModel> LoginAsync(
        LoginModel model, 
        string? ipAddress, 
        string? userAgent, 
        CancellationToken cancellationToken = default)
    {
        // 1. Kullanıcıyı kullanıcı adına göre bul
        var user = await _userManager.FindByNameAsync(model.Username);
        
        if (user == null)
        {
            _logger.Warning("Login başarısız: Kullanıcı bulunamadı - {Username}", model.Username);
            throw new NotFoundException(LanguageTexts.InvalidUsernameOrPassword);
        }

        // 2. Kullanıcı onaylı mı kontrol et
        if (!user.IsApproved)
        {
            _logger.Warning("Login başarısız: Kullanıcı onaylı değil - {Username}", model.Username);
            throw new NotFoundException(LanguageTexts.UserNotApproved);
        }

        // 3. Şifre doğrulaması
        var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        
        if (!passwordValid)
        {
            _logger.Warning("Login başarısız: Geçersiz şifre - {Username}", model.Username);
            throw new NotFoundException(LanguageTexts.InvalidUsernameOrPassword);
        }

        // 4. Kullanıcı detaylarını yükle (Language, Organization, Person)
        var userWithDetails = await _dbContext.Users
            .Include(x => x.Language)
            .Include(x => x.Organization)
            .Include(x => x.Person)
            .FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);

        if (userWithDetails == null)
        {
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }

        // 5. JWT Token oluştur
        var token = GenerateJwtToken(userWithDetails);
        var expiresAt = DateTime.UtcNow.AddHours(GetTokenExpirationHours());

        // 6. UserSession kaydı oluştur
        var userSession = new UserSession
        {
            Id = Guid.CreateVersion7(),
            IpAddress = ipAddress,
            AgentInfo = userAgent,
            CreationTime = DateTime.UtcNow,
            ExpiresAt = expiresAt,
            Creator = userWithDetails
        };

        _dbContext.Set<UserSession>().Add(userSession);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.Information("Login başarılı - User: {Username}, SessionId: {SessionId}", 
            userWithDetails.UserName, userSession.Id);

        // 7. Response oluştur
        return new LoginResponseModel
        {
            Token = token,
            ExpiresAt = expiresAt,
            UserId = userWithDetails.Id,
            Username = userWithDetails.UserName ?? string.Empty,
            Email = userWithDetails.Email ?? string.Empty,
            LanguageId = userWithDetails.LanguageId,
            OrganizationId = userWithDetails.OrganizationId,
            PersonId = userWithDetails.PersonId
        };
    }

    /// <summary>
    /// JWT token oluşturur.
    /// </summary>
    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new("LanguageId", user.LanguageId.ToString()),
            new("OrganizationId", user.OrganizationId.ToString()),
            new("PersonId", user.PersonId.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey configuration is missing")));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(GetTokenExpirationHours()),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Token geçerlilik süresini saat cinsinden döndürür.
    /// </summary>
    private double GetTokenExpirationHours()
    {
        var value = _configuration["Jwt:ExpirationHours"];
        return string.IsNullOrEmpty(value) ? 24 : double.Parse(value);
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Struvio.Application.Services.Abstract;
using Struvio.Common.Models;
using Struvio.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Struvio.Application.Services.Concrete;

/// <summary>
/// Authentication servisinin implementasyonu.
/// Kullanıcı girişi, JWT token oluşturma ve oturum yönetimi işlemlerini gerçekleştirir.
/// </summary>
public class AuthService(
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext dbContext,
    IConfiguration configuration,
    IStruvioLogger logger) : IAuthService
{

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
        var user = await userManager.FindByNameAsync(model.Username);
        
        if (user == null)
        {
            logger.Warning("Login başarısız: Kullanıcı bulunamadı - {Username}", model.Username);
            throw new NotFoundException(LanguageTexts.InvalidUsernameOrPassword);
        }

        // 2. Kullanıcı onaylı mı kontrol et
        if (!user.IsApproved)
        {
            logger.Warning("Login başarısız: Kullanıcı onaylı değil - {Username}", model.Username);
            throw new NotFoundException(LanguageTexts.UserNotApproved);
        }

        // 3. Şifre doğrulaması
        var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);
        
        if (!passwordValid)
        {
            logger.Warning("Login başarısız: Geçersiz şifre - {Username}", model.Username);
            throw new NotFoundException(LanguageTexts.InvalidUsernameOrPassword);
        }

        // 4. Kullanıcı detaylarını yükle (Language, Organization, Person)
        var userWithDetails = await dbContext.Users
            .Include(x => x.Language)
            .Include(x => x.Organization)
            .Include(x => x.Person)
            .FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);

        if (userWithDetails == null)
        {
            logger.Error("Login başarısız: Kullanıcı detayları yüklenemedi - {Username}", model.Username);
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }

        var session = await dbContext.Set<UserSession>().FirstOrDefaultAsync(x => x.Creator.Id == user.Id, cancellationToken);
        if (session != null)
        {

            // Açık oturum varsa, oturum geçmişine kaydet ve oturumu sil
            await dbContext.Set<UserSessionHistory>().AddAsync(new UserSessionHistory
            {
                Id = Guid.CreateVersion7(),
                UserSessionId = session.Id,
                IpAddress = session.IpAddress,
                AgentInfo = session.AgentInfo,
                CreationTime = session.CreationTime,
                LastModificationTime = DateTime.UtcNow,
                CreatorId = session.Creator.Id,
                LastModifierId = user.Id,
                LogoutType = LogoutType.Forced
            }, cancellationToken);

            dbContext.Set<UserSession>().Remove(session);
            await dbContext.SaveChangesAsync(cancellationToken);
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

        dbContext.Set<UserSession>().Add(userSession);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.Information("Login başarılı - User: {Username}, SessionId: {SessionId}", 
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
            Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey configuration is missing")));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
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
        var value = configuration["Jwt:ExpirationHours"];
        return string.IsNullOrEmpty(value) ? 24 : double.Parse(value);
    }
}

using Struvio.Application.Services.Abstract;
using Struvio.Common.Models;
using Struvio.Common.ValueObjects;

namespace Struvio.UI.Web.Api.Controllers;

/// <summary>
/// Kimlik doğrulama işlemlerini yöneten controller.
/// Login, logout ve token yenileme gibi işlemleri sağlar.
/// </summary>
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService, IStruvioLogger logger) : ControllerBase
{

    /// <summary>
    /// Kullanıcı girişi yapar ve JWT token döner.
    /// </summary>
    /// <param name="model">Login modeli (kullanıcı adı ve şifre)</param>
    /// <param name="cancellationToken">İptal token</param>
    /// <returns>JWT token ve kullanıcı bilgileri</returns>
    /// <response code="200">Login başarılı, token döndürüldü</response>
    /// <response code="400">Geçersiz model</response>
    /// <response code="401">Geçersiz kullanıcı adı veya şifre</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginModel model, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                
                return BadRequest(ApiResponse.Error(errors));
            }

            var ipAddress = HttpContext.GetIpAddress();
            var userAgent = HttpContext.GetUserAgent();

            var response = await authService.LoginAsync(model, ipAddress, userAgent, cancellationToken);

            logger.Information("Login endpoint başarılı - Username: {Username}", model.Username);

            return Ok(ApiResponse<LoginResponseModel>.Success(response, "Login başarılı"));
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Login endpoint hatası - Username: {Username}", model.Username);
            return Unauthorized(ApiResponse.Error(ex.Message));
        }
    }
}

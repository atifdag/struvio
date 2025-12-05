using Struvio.Common.Models;

namespace Struvio.Application.Services.Abstract;

/// <summary>
/// Authentication servisi arayüzü.
/// Kimlik doğrulama işlemlerini tanımlar.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Kullanıcı girişi yapar.
    /// </summary>
    /// <param name="model">Login modeli (kullanıcı adı ve şifre)</param>
    /// <param name="ipAddress">İstek yapan IP adresi</param>
    /// <param name="userAgent">İstek yapan kullanıcı agent bilgisi</param>
    /// <param name="cancellationToken">İptal token</param>
    /// <returns>Login response modeli (JWT token ve kullanıcı bilgileri)</returns>
    Task<LoginResponseModel> LoginAsync(LoginModel model, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);
}

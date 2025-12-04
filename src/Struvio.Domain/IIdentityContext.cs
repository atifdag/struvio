namespace Struvio.Domain;

/// <summary>
/// Kimlik doğrulama bağlamı arayüzü. Oturum bilgilerini sağlar.
/// </summary>
public interface IIdentityContext
{
    /// <summary>
    /// Oturumdaki kullanıcının kimlik numarasını getirir.
    /// </summary>
    /// <returns>Kullanıcı kimlik numarası</returns>
    Task<Guid> GetUserIdAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Oturumdaki kullanıcının dil kimlik numarasını getirir.
    /// </summary>
    /// <returns>Dil kimlik numarası</returns>
    Task<Guid> GetLanguageIdAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Oturumdaki kullanıcının organizasyon kimlik numarasını getirir.
    /// </summary>
    /// <returns>Organizasyon kimlik numarası</returns>
    Task<Guid> GetOrganizationIdAsync(CancellationToken cancellationToken = default);
}

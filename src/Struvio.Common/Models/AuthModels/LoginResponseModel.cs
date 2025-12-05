namespace Struvio.Common.Models.AuthModels;

/// <summary>
/// Login işlemi sonucunda dönen response modeli.
/// JWT token ve kullanıcı bilgilerini içerir.
/// </summary>
public class LoginResponseModel
{
    /// <summary>
    /// JWT access token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Token'ın geçerlilik süresi (UTC)
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Kullanıcının benzersiz kimlik numarası
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Kullanıcı adı
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// E-posta adresi
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcının dil ID'si
    /// </summary>
    public Guid LanguageId { get; set; }

    /// <summary>
    /// Kullanıcının organizasyon ID'si
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Kullanıcının kişi bilgisi ID'si
    /// </summary>
    public Guid PersonId { get; set; }
}

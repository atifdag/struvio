

namespace Struvio.Domain.Exceptions;

/// <summary>
/// Kimlik bulunamadığında fırlatılan istisna sınıfı. Kullanıcı doğrulaması başarısız olduğunda kullanılır.
/// </summary>
public class IdentityNotFoundException : BaseApplicationException
{
    /// <summary>
    /// Varsayılan mesajla yeni bir <see cref="IdentityNotFoundException"/> örneği oluşturur.
    /// </summary>
    public IdentityNotFoundException() : base(LanguageTexts.IdentityUserNotFound)
    {
    }

    /// <summary>
    /// Özel mesajla yeni bir <see cref="IdentityNotFoundException"/> örneği oluşturur.
    /// </summary>
    /// <param name="message">İstisna mesajı</param>
    public IdentityNotFoundException(string message) : base(message)
    {

    }

}

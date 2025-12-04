namespace Struvio.Domain.Exceptions;

/// <summary>
/// Tüm uygulama özel istisnaları için temel sınıf. Özel hata durumları için kullanılır.
/// </summary>
public abstract class BaseApplicationException(string message) : ApplicationException(message)
{
}

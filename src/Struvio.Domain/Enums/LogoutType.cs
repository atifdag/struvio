namespace Struvio.Domain.Enums;

/// <summary>
/// Oturum kapanış türlerini belirten numaralandırma.
/// </summary>
public enum LogoutType
{
    /// <summary>
    /// Normal oturum kapama - Kullanıcı isteyerek oturumu sonlandırdığında
    /// </summary>
    Normal = 1,

    /// <summary>
    /// Zorla oturum kapama - Sistemsel bir neden ya da yönetici tarafından oturum sonlandırıldığında
    /// </summary>
    Forced = 2,

    /// <summary>
    /// Zaman aşımı - Oturum süresi dolduğunda
    /// </summary>
    TimeOut = 3,
}
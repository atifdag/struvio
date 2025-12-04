namespace Struvio.Domain.Entities;

/// <summary>
/// Kullanıcı oturum geçmişi varlığı. Sonlanmış kullanıcı oturumlarını ve oturum kapanış bilgilerini temsil eder.
/// </summary>
public class UserSessionHistory : IEntity
{
    /// <summary>
    /// Oturum geçmişinin benzersiz kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// İlgili kullanıcı oturumunun kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid UserSessionId { get; set; }
    
    /// <summary>
    /// Oturumun açıldığı IP adresini alır veya ayarlar.
    /// </summary>
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// Oturumun açıldığı cihaz/tarayıcı bilgisini alır veya ayarlar.
    /// </summary>
    public string? AgentInfo { get; set; }
    
    /// <summary>
    /// Oturumun oluşturulma zamanını alır veya ayarlar.
    /// </summary>
    public DateTime CreationTime { get; set; }
    
    /// <summary>
    /// Oturumun sonlandırıldığı zamanı alır veya ayarlar.
    /// </summary>
    public DateTime LastModificationTime { get; set; }
    
    /// <summary>
    /// Oturumu açan kullanıcının kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid CreatorId { get; set; }
    
    /// <summary>
    /// Oturumu sonlandıran kullanıcının kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid LastModifierId { get; set; }
    
    /// <summary>
    /// Oturum kapanış türünü alır veya ayarlar.
    /// </summary>
    public LogoutType LogoutType { get; set; }
}
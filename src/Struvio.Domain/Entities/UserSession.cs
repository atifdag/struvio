namespace Struvio.Domain.Entities;

/// <summary>
/// Kullanıcı oturumu varlığı. Aktif kullanıcı oturumlarını temsil eder.
/// </summary>
public class UserSession : IEntity
{
    /// <summary>
    /// Oturumun benzersiz kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid Id { get; set; }
    
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
    /// Oturumun süresinin dolacağı zamanı alır veya ayarlar.
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    
    /// <summary>
    /// Oturumu açan kullanıcıyı alır veya ayarlar.
    /// </summary>
    public virtual ApplicationUser Creator { get; set; } = null!;
}

namespace Struvio.Domain.Entities;

/// <summary>
/// Kullanıcı-Organizasyon-Rol ilişki satırı. Kullanıcıların organizasyonlardaki rollerini temsil eder.
/// </summary>
public class UserOrganizationRoleLine : IdentityUserRole<Guid>, IHistoryEntity
{
    /// <summary>
    /// İlişkinin benzersiz kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// İlişkinin versiyon numarasını alır veya ayarlar.
    /// </summary>
    public long Version { get; set; }
    
    /// <summary>
    /// İlişkinin oluşturulma zamanını alır veya ayarlar.
    /// </summary>
    public DateTime CreationTime { get; set; }
    
    /// <summary>
    /// İlişkinin son değiştirilme zamanını alır veya ayarlar.
    /// </summary>
    public DateTime LastModificationTime { get; set; }
    
    /// <summary>
    /// İlişkili kullanıcıyı alır veya ayarlar.
    /// </summary>
    public virtual ApplicationUser User { get; set; } = null!;
    
    /// <summary>
    /// İlişkili organizasyonu alır veya ayarlar.
    /// </summary>
    public virtual Organization Organization { get; set; } = null!;
    
    /// <summary>
    /// İlişkili rolü alır veya ayarlar.
    /// </summary>
    public virtual ApplicationRole Role { get; set; } = null!;
    
    /// <summary>
    /// İlişkiyi oluşturan kullanıcıyı alır veya ayarlar.
    /// </summary>
    public virtual ApplicationUser Creator { get; set; } = null!;
    
    /// <summary>
    /// İlişkiyi son değiştiren kullanıcıyı alır veya ayarlar.
    /// </summary>
    public virtual ApplicationUser LastModifier { get; set; } = null!;


  
   
}


namespace Struvio.Domain.Entities;

/// <summary>
/// Yetki varlığı. Sistemdeki yetkileri ve erişim izinlerini temsil eder.
/// </summary>
public class Permission : IHistoryEntity
{
    /// <summary>
    /// Yetkinin benzersiz kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Yetkinin benzersiz kodunu alır veya ayarlar.
    /// </summary>
    public string Code { get; set; } = null!;
    
    /// <summary>
    /// Yetkinin ait olduğu controller adını alır veya ayarlar.
    /// </summary>
    public string? ControllerName { get; set; }
    
    /// <summary>
    /// Yetkinin ait olduğu action adını alır veya ayarlar.
    /// </summary>
    public string? ActionName { get; set; }
    
    /// <summary>
    /// Yetkinin ait olduğu yolu alır veya ayarlar.
    /// </summary>
    public string? Path { get; set; }
    
    /// <summary>
    /// Yetkinin sıra numarasını alır veya ayarlar.
    /// </summary>
    public long SequenceNumber { get; set; }
    
    /// <summary>
    /// Yetkinin onaylı olup olmadığını belirtir.
    /// </summary>
    public bool IsApproved { get; set; }
    
    /// <summary>
    /// Yetkinin versiyon numarasını alır veya ayarlar.
    /// </summary>
    public long Version { get ; set ; }
    
    /// <summary>
    /// Yetkinin oluşturulma zamanını alır veya ayarlar.
    /// </summary>
    public DateTime CreationTime { get ; set ; }
    
    /// <summary>
    /// Yetkinin son değiştirilme zamanını alır veya ayarlar.
    /// </summary>
    public DateTime LastModificationTime { get ; set ; }
    
    /// <summary>
    /// Yetkiyi oluşturan kullanıcıyı alır veya ayarlar.
    /// </summary>
    public virtual ApplicationUser Creator { get; set; } = null!;
    
    /// <summary>
    /// Yetkiyi son değiştiren kullanıcıyı alır veya ayarlar.
    /// </summary>
    public virtual ApplicationUser LastModifier { get; set; } = null!;
    
    /// <summary>
    /// Yetkiye ait rol-yetki ilişkilerini alır veya ayarlar.
    /// </summary>
    public virtual ICollection<RolePermissionLine> RolePermissionLines { get; set; } = [];


}

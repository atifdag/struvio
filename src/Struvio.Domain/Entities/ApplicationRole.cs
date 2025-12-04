namespace Struvio.Domain.Entities;

/// <summary>
/// Uygulama rolü varlığı. Kullanıcı yetkilendirme sisteminde rol tanımlarını temsil eder.
/// </summary>
public class ApplicationRole : IdentityRole<Guid>, IOrganizationalEntity
{
    /// <summary>
    /// Rolün benzersiz kodunu alır veya ayarlar.
    /// </summary>
    public string Code { get; set; } = null!;
    
    /// <summary>
    /// Rolün sıra numarasını alır veya ayarlar.
    /// </summary>
    public long SequenceNumber { get; set; }
    
    /// <summary>
    /// Rolün onaylı olup olmadığını belirtir.
    /// </summary>
    public bool IsApproved { get; set; }
    
    /// <summary>
    /// Rolün versiyon numarasını alır veya ayarlar.
    /// </summary>
    public long Version { get; set; }
    
    /// <summary>
    /// Rolün oluşturulma zamanını alır veya ayarlar.
    /// </summary>
    public DateTime CreationTime { get; set; }
    
    /// <summary>
    /// Rolün son değiştirilme zamanını alır veya ayarlar.
    /// </summary>
    public DateTime LastModificationTime { get; set; }
    
    /// <summary>
    /// Rolün bağlı olduğu organizasyonu alır veya ayarlar.
    /// </summary>
    public virtual Organization Organization { get; set; } = null!;
    
    /// <summary>
    /// Rolü oluşturan kullanıcıyı alır veya ayarlar.
    /// </summary>
    public virtual ApplicationUser Creator { get; set; } = null!;
    
    /// <summary>
    /// Rolü son değiştiren kullanıcıyı alır veya ayarlar.
    /// </summary>
    public virtual ApplicationUser LastModifier { get; set; } = null!;
    
    /// <summary>
    /// Role ait yetki satırlarını alır veya ayarlar.
    /// </summary>
    public virtual ICollection<RolePermissionLine> RolePermissionLines { get; set; } = [];
    
    /// <summary>
    /// Kullanıcı-organizasyon-rol ilişki satırlarını alır veya ayarlar.
    /// </summary>
    public virtual ICollection<UserOrganizationRoleLine> UserOrganizationRoleLines { get; set; } = [];
}

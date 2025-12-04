namespace Struvio.Domain.Entities;

/// <summary>
/// Uygulama kullanıcı varlığı. Sistemdeki kullanıcıları temsil eder ve kimlik doğrulama işlemlerinde kullanılır.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>, IOrganizationalEntity
{
    /// <summary>
    /// Kullanıcının kişi bilgisi kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid PersonId { get; set; }
    
    /// <summary>
    /// Kullanıcının kişi bilgisini alır veya ayarlar.
    /// </summary>
    public virtual Person Person { get; set; } = null!;
    
    /// <summary>
    /// Kullanıcının organizasyon kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// Kullanıcının organizasyonunu alır veya ayarlar.
    /// </summary>
    public virtual Organization Organization { get; set; } = null!;
    
    /// <summary>
    /// Kullanıcının dil kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid LanguageId { get; set; }
    
    /// <summary>
    /// Kullanıcının dilini alır veya ayarlar.
    /// </summary>
    public virtual Language Language { get; set; } = null!;
    
    /// <summary>
    /// Kullanıcının sıra numarasını alır veya ayarlar.
    /// </summary>
    public long SequenceNumber { get; set; }

    /// <summary>
    /// Kullanıcının onaylı olup olmadığını belirtir.
    /// </summary>
    public bool IsApproved { get; set; }
    
    /// <summary>
    /// Kullanıcının versiyon numarasını alır veya ayarlar.
    /// </summary>
    public long Version { get; set; }
   
    /// <summary>
    /// Kullanıcının oluşturulma zamanını alır veya ayarlar.
    /// </summary>
    public DateTime CreationTime { get; set; }
    
    /// <summary>
    /// Kullanıcının son değiştirilme zamanını alır veya ayarlar.
    /// </summary>
    public DateTime LastModificationTime { get; set; }
   
    /// <summary>
    /// Kullanıcıyı oluşturan kullanıcıyı alır veya ayarlar.
    /// </summary>
    public virtual ApplicationUser Creator { get; set; } = null!;
    
    /// <summary>
    /// Kullanıcıyı son değiştiren kullanıcıyı alır veya ayarlar.
    /// </summary>
    public virtual ApplicationUser LastModifier { get; set; } = null!;
    
    /// <summary>
    /// Kullanıcının organizasyon-rol ilişkilerini alır veya ayarlar.
    /// </summary>
    public ICollection<UserOrganizationRoleLine> UserOrganizationRoleLines { get; set; } = [];
    
    /// <summary>
    /// Kullanıcının oluşturduğu oturumları alır veya ayarlar.
    /// </summary>
    public ICollection<UserSession> CreatedUserSessions { get; set; } = [];

    /// <summary>
    /// Bu kullanıcı tarafından oluşturulan kullanıcıları alır veya ayarlar.
    /// </summary>
    public ICollection<ApplicationUser> CreatedUsers { get; set; } = [];
    
    /// <summary>
    /// Bu kullanıcı tarafından son değiştirilen kullanıcıları alır veya ayarlar.
    /// </summary>
    public ICollection<ApplicationUser> LastModifiedUsers { get; set; } = [];

    /// <summary>
    /// Bu kullanıcı tarafından oluşturulan rolleri alır veya ayarlar.
    /// </summary>
    public ICollection<ApplicationRole> CreatedRoles { get; set; } = [];
    
    /// <summary>
    /// Bu kullanıcı tarafından son değiştirilen rolleri alır veya ayarlar.
    /// </summary>
    public ICollection<ApplicationRole> LastModifiedRoles { get; set; } = [];

    /// <summary>
    /// Bu kullanıcı tarafından oluşturulan yetkileri alır veya ayarlar.
    /// </summary>
    public ICollection<Permission> CreatedPermissions { get; set; } = [];
    
    /// <summary>
    /// Bu kullanıcı tarafından son değiştirilen yetkileri alır veya ayarlar.
    /// </summary>
    public ICollection<Permission> LastModifiedPermissions { get; set; } = [];

    /// <summary>
    /// Bu kullanıcı tarafından oluşturulan rol-yetki ilişkilerini alır veya ayarlar.
    /// </summary>
    public ICollection<RolePermissionLine> CreatedRolePermissionLines { get; set; } = [];
    
    /// <summary>
    /// Bu kullanıcı tarafından son değiştirilen rol-yetki ilişkilerini alır veya ayarlar.
    /// </summary>
    public ICollection<RolePermissionLine> LastModifiedRolePermissionLines { get; set; } = [];

    /// <summary>
    /// Bu kullanıcı tarafından oluşturulan kullanıcı-organizasyon-rol ilişkilerini alır veya ayarlar.
    /// </summary>
    public ICollection<UserOrganizationRoleLine> CreatedUserOrganizationRoleLines { get; set; } = [];
    
    /// <summary>
    /// Bu kullanıcı tarafından son değiştirilen kullanıcı-organizasyon-rol ilişkilerini alır veya ayarlar.
    /// </summary>
    public ICollection<UserOrganizationRoleLine> LastModifiedUserOrganizationRoleLines { get; set; } = [];







}

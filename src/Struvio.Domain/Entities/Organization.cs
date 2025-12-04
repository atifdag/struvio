namespace Struvio.Domain.Entities;

/// <summary>
/// Organizasyon varlığı. Sistemdeki kuruluşları temsil eder ve çok kiracılı yapıyı sağlar.
/// </summary>
public class Organization
{
    /// <summary>
    /// Organizasyonun benzersiz kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Organizasyonun benzersiz kodunu alır veya ayarlar.
    /// </summary>
    public string Code { get; set; } = null!;
    
    /// <summary>
    /// Organizasyonun adını alır veya ayarlar.
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Organizasyonun API anahtarını alır veya ayarlar.
    /// </summary>
    public string ApiKey { get; set; } = null!;
    
    /// <summary>
    /// Organizasyonun API şifresini alır veya ayarlar.
    /// </summary>
    public string ApiPassword { get; set; } = null!;
    
    /// <summary>
    /// Organizasyonun açıklamasını alır veya ayarlar.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Organizasyonun sıra numarasını alır veya ayarlar.
    /// </summary>
    public long SequenceNumber { get; set; }
    
    /// <summary>
    /// Organizasyonun onaylı olup olmadığını belirtir.
    /// </summary>
    public bool IsApproved { get; set; }
    
    /// <summary>
    /// Organizasyonun versiyon numarasını alır veya ayarlar.
    /// </summary>
    public long Version { get; set; }
    
    /// <summary>
    /// Organizasyonun oluşturulma zamanını alır veya ayarlar.
    /// </summary>
    public DateTime CreationTime { get; set; }
    
    /// <summary>
    /// Organizasyonun son değiştirilme zamanını alır veya ayarlar.
    /// </summary>
    public DateTime LastModificationTime { get; set; }
    
    /// <summary>
    /// Organizasyonu oluşturan kullanıcının kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid CreatorId { get; set; }
    
    /// <summary>
    /// Organizasyonu son değiştiren kullanıcının kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid LastModifierId { get; set; }

    /// <summary>
    /// Organizasyona ait kullanıcıları alır veya ayarlar.
    /// </summary>
    public ICollection<ApplicationUser> Users { get; set; } = [];
    
    /// <summary>
    /// Kullanıcı-organizasyon-rol ilişkilerini alır veya ayarlar.
    /// </summary>
    public virtual ICollection<UserOrganizationRoleLine> UserOrganizationRoleLines { get; set; } = [];
    
    /// <summary>
    /// Organizasyona ait rolleri alır veya ayarlar.
    /// </summary>
    public virtual ICollection<ApplicationRole> Roles { get; set; } = [];


}

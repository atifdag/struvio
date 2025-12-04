namespace Struvio.Domain.Entities;

/// <summary>
/// Kişi varlığı. Sistemdeki gerçek kişileri temsil eder.
/// </summary>
public class Person
{
    /// <summary>
    /// Kişinin benzersiz kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Kişinin kimlik numarasını (TC Kimlik No vb.) alır veya ayarlar.
    /// </summary>
    public string IdentityNumber { get; set; } = null!;
    
    /// <summary>
    /// Kişinin adını alır veya ayarlar.
    /// </summary>
    public string FirstName { get; set; } = null!;
    
    /// <summary>
    /// Kişinin soyadını alır veya ayarlar.
    /// </summary>
    public string LastName { get; set; } = null!;
    
    /// <summary>
    /// Kişinin görünen adını alır (Ad + Soyad).
    /// </summary>
    public string DisplayName => $"{FirstName} {LastName}";
    
    /// <summary>
    /// Kişinin onaylı olup olmadığını belirtir.
    /// </summary>
    public bool IsApproved { get; set; }
    
    /// <summary>
    /// Kişinin versiyon numarasını alır veya ayarlar.
    /// </summary>
    public long Version { get; set; }
    
    /// <summary>
    /// Kişinin oluşturulma zamanını alır veya ayarlar.
    /// </summary>
    public DateTime CreationTime { get; set; }
    
    /// <summary>
    /// Kişinin son değiştirilme zamanını alır veya ayarlar.
    /// </summary>
    public DateTime LastModificationTime { get; set; }
    
    /// <summary>
    /// Kişiyi oluşturan kullanıcının kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid CreatorId { get; set; }
    
    /// <summary>
    /// Kişiyi son değiştiren kullanıcının kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid LastModifierId { get; set; }
    
    /// <summary>
    /// Kişiye ait kullanıcı hesabını alır veya ayarlar.
    /// </summary>
    public virtual ApplicationUser? User { get; set; }

    
}

namespace Struvio.Domain.Entities;

/// <summary>
/// Dil varlığı. Sistemde desteklenen dilleri temsil eder.
/// </summary>
public class Language
{
    /// <summary>
    /// Dilin benzersiz kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Dilin kısa kodunu alır veya ayarlar (örn: "tr", "en").
    /// </summary>
    public string ShortCode { get; set; } = null!;
    
    /// <summary>
    /// Dilin kodunu alır veya ayarlar (örn: "tr-TR", "en-US").
    /// </summary>
    public string Code { get; set; } = null!;
    
    /// <summary>
    /// Dilin adını alır veya ayarlar.
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Dilin sıra numarasını alır veya ayarlar.
    /// </summary>
    public long SequenceNumber { get; set; }
    
    /// <summary>
    /// Dilin onaylı olup olmadığını belirtir.
    /// </summary>
    public bool IsApproved { get; set; }
    
    /// <summary>
    /// Dilin versiyon numarasını alır veya ayarlar.
    /// </summary>
    public long Version { get; set; }
    
    /// <summary>
    /// Dilin oluşturulma zamanını alır veya ayarlar.
    /// </summary>
    public DateTime CreationTime { get; set; }
    
    /// <summary>
    /// Dilin son değiştirilme zamanını alır veya ayarlar.
    /// </summary>
    public DateTime LastModificationTime { get; set; }
    
    /// <summary>
    /// Dili oluşturan kullanıcının kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid CreatorId { get; set; }
    
    /// <summary>
    /// Dili son değiştiren kullanıcının kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid LastModifierId { get; set; }
    
    /// <summary>
    /// Bu dili kullanan kullanıcıları alır veya ayarlar.
    /// </summary>
    public virtual ICollection<ApplicationUser> Users { get; set; } = [];

}

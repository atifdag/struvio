namespace Struvio.Common;

/// <summary>
/// CRUD işlemlerinde kullanılan modeller için arayüz
/// </summary>
public interface ICrudModel
{
    /// <summary>
    /// Birincil anahtar
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Sıra No
    /// </summary>
    public long SequenceNumber { get; set; }

    /// <summary>
    /// Onaylı kayıt mı?
    /// </summary>
    public bool IsApproved { get; set; }

    /// <summary>
    /// Sürüm No
    /// </summary>
    public long Version { get; set; }

    /// <summary>
    /// Oluşturulma zamanı
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Son değişiklik zamanı
    /// </summary>
    public DateTime LastModificationTime { get; set; }

    /// <summary>
    /// Oluşturan kullanıcı
    /// </summary>
    public IdName? Creator { get; set; }

    /// <summary>
    /// Son değiştiren kullanıcı
    /// </summary>
    public IdName? LastModifier { get; set; }


}

namespace Struvio.Domain;

/// <summary>
/// Geçmiş kaydı tutulan varlıklar için arayüz. Versiyon ve değişiklik bilgilerini içerir.
/// </summary>
public interface IHistoryEntity : IEntity
{
    /// <summary>
    /// Varlığın versiyon numarasını alır veya ayarlar.
    /// </summary>
    long Version { get; set; }
    
    /// <summary>
    /// Varlığın oluşturulma zamanını alır veya ayarlar.
    /// </summary>
    DateTime CreationTime { get; set; }
    
    /// <summary>
    /// Varlığın son değiştirilme zamanını alır veya ayarlar.
    /// </summary>
    DateTime LastModificationTime { get; set; }
    
    /// <summary>
    /// Varlığı oluşturan kullanıcıyı alır veya ayarlar.
    /// </summary>
    ApplicationUser Creator { get; set; }
    
    /// <summary>
    /// Varlığı son değiştiren kullanıcıyı alır veya ayarlar.
    /// </summary>
    ApplicationUser LastModifier { get; set; }

}

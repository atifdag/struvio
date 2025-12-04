namespace Struvio.Domain;

/// <summary>
/// Organizasyona bağlı varlıklar için arayüz. Geçmiş kaydına ek olarak organizasyon ilişkisi içerir.
/// </summary>
public interface IOrganizationalEntity : IHistoryEntity
{
    /// <summary>
    /// Varlığın bağlı olduğu organizasyonu alır veya ayarlar.
    /// </summary>
    Organization Organization { get; set; }

}

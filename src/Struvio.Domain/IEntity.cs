namespace Struvio.Domain;

/// <summary>
/// Temel varlık arayüzü. Tüm domain nesneleri için kimlik özelliği sağlar.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Varlığın benzersiz kimlik numarasını alır veya ayarlar.
    /// </summary>
    Guid Id { get; set; }

}

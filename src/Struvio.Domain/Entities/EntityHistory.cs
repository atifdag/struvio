namespace Struvio.Domain.Entities;

/// <summary>
/// Varlık geçmişi kaydı. Tüm varlık değişikliklerini izlemek için kullanılır.
/// </summary>
public class History
{
    /// <summary>
    /// Geçmiş kaydının benzersiz kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Varlık adını alır veya ayarlar.
    /// </summary>
    public string EntityName { get; set; } = null!;
    
    /// <summary>
    /// İzlenen varlığın satır kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid RowId { get; set; }
    
    /// <summary>
    /// İşlemi yapan kullanıcının kimlik numarasını alır veya ayarlar.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Varlığın versiyon numarasını alır veya ayarlar.
    /// </summary>
    public long Version { get; set; }
    
    /// <summary>
    /// Varlığın silinip silinmediğini belirtir.
    /// </summary>
    public bool IsDeleted { get; set; }
    
    /// <summary>
    /// Varlığın tam veri içeriğini JSON formatında alır veya ayarlar.
    /// </summary>
    public JsonDocument? Data { get; set; }
    
    /// <summary>
    /// Varlıkta değişen alanları JSON formatında alır veya ayarlar.
    /// </summary>
    public JsonDocument? ChangedData { get; set; }
    
    /// <summary>
    /// İşlemin gerçekleştiği zamanı alır veya ayarlar.
    /// </summary>
    public DateTime TransactionTime { get; set; }
}

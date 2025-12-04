namespace Struvio.Persistence.Utils;

/// <summary>
/// Persistence katmanı için genişletme metodlarını içeren statik sınıf.
/// Varlık değişiklik tespiti ve JSON dönüşüm işlemleri için yardımcı metodlar sağlar.
/// </summary>
internal static class PersistenceExtensions
{
    extension(EntityEntry change)
    {
        /// <summary>
        /// Varlıktaki değişiklikleri tespit eder ve JSON formatında döndürür.
        /// </summary>
        /// <returns>Değişiklik varsa JSON dökümanı, yoksa null</returns>
        internal JsonDocument? EntityChangeDetection()
        {
            IEnumerable<EntityChangeLog> changeLogList = from p in change.OriginalValues.Properties
                                                         let originalValue = change.OriginalValues[p.Name] is not null ? change.OriginalValues[p.Name]!.ToString() : null
                                                         let currentValue = change.CurrentValues[p.Name] is not null ? change.CurrentValues[p.Name]!.ToString() : null
                                                         where originalValue != currentValue
                                                         select new EntityChangeLog(p.Name, originalValue, currentValue);
            return changeLogList.Any() ? JsonSerializer.SerializeToDocument(changeLogList) : null;
        }
    }

    /// <summary>
    /// Varsayılan JSON serileştirme seçenekleri.
    /// </summary>
    internal static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = false
    };

    /// <summary>
    /// Geçmiş kayıtları için JSON serileştirme seçenekleri.
    /// </summary>
    internal static readonly JsonSerializerOptions HistorySerializerOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = false,
        Converters = {
            new HistoryConverterFactory()
        }
    };

    /// <summary>
    /// Metin şifreleme ve şifre çözme için değer dönüştürücü.
    /// Veritabanında hassas verileri şifrelemek için kullanılır.
    /// </summary>
    internal static ValueConverter<string, string> EncryptionValueConverter = new(v => v.Encrypt(), v => v.Decrypt());

    /// <summary>
    /// Varlığı geçmiş kaydı oluşturmak için JSON formatına dönüştürür.
    /// </summary>
    /// <param name="entity">Dönüştürülecek varlık</param>
    /// <returns>JSON dökümanı</returns>
    internal static JsonDocument ToCreateHistoryAsJson(this object entity)
    {
        return JsonSerializer.SerializeToDocument(entity, HistorySerializerOptions);
    }
}

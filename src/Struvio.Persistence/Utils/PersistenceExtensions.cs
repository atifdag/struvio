namespace Struvio.Persistence.Utils;

/// <summary>
/// Persistence katmanı için genişletme metodlarını içeren statik sınıf.
/// Varlık değişiklik tespiti ve JSON dönüşüm işlemleri için yardımcı metodlar sağlar.
/// </summary>
internal static class PersistenceExtensions
{
    /// <summary>
    /// Varlıktaki değişiklikleri tespit eder ve JSON formatında döndürür.
    /// </summary>
    /// <param name="change">Değişiklik kaydı</param>
    /// <returns>Değişiklik varsa JSON dökümanı, yoksa null</returns>
    internal static JsonDocument? EntityChangeDetection(this EntityEntry change)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        writer.WriteStartArray();
        bool hasChanges = false;

        foreach (var prop in change.OriginalValues.Properties)
        {
            var originalValue = change.OriginalValues[prop.Name]?.ToString();
            var currentValue = change.CurrentValues[prop.Name]?.ToString();

            if (originalValue != currentValue)
            {
                hasChanges = true;
                writer.WriteStartObject();
                writer.WriteString("PropertyName", prop.Name);
                writer.WriteString("OldValue", originalValue);
                writer.WriteString("NewValue", currentValue);
                writer.WriteEndObject();
            }
        }

        writer.WriteEndArray();
        writer.Flush();

        if (!hasChanges) return null;

        stream.Position = 0;
        return JsonDocument.Parse(stream);
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

namespace Struvio.Persistence.Utils;

/// <summary>
/// Persistence katmanı için sabit değerleri içeren sınıf.
/// JSON serileştirme seçenekleri ve alan uzunluk sabitleri burada tanımlanır.
/// </summary>
internal class PersistenceConstants
{
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
    /// HistoryConverterFactory kullanarak özel dönüşüm sağlar.
    /// </summary>
    internal static readonly JsonSerializerOptions HistorySerializerOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = false,
        Converters = {
            new HistoryConverterFactory()
        }
    };

    /// <summary>Maksimum 11 karakter uzunluğu (örn: telefon numarası)</summary>
    internal const int Max11Lenght = 11;
    
    /// <summary>Maksimum 50 karakter uzunluğu (örn: kullanıcı adı)</summary>
    internal const int Max50Lenght = 50;
    
    /// <summary>Maksimum 256 karakter uzunluğu (örn: e-posta, kod alanları)</summary>
    internal const int Max256Lenght = 256;
    
    /// <summary>Maksimum 512 karakter uzunluğu (örn: güvenlik anahtarları)</summary>
    internal const int Max512Lenght = 256;
    
    /// <summary>Maksimum 2000 karakter uzunluğu (örn: açıklama alanları)</summary>
    internal const int Max2000Lenght = 2000;
}

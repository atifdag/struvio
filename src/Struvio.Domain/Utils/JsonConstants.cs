namespace Struvio.Domain.Utils;

public class JsonConstants
{
    /// <summary>
    /// Json serileştirme ve deserileştirme işlemlerinin seçenekleri
    /// </summary>
    public static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        // Döngüye neden olan gezinti özelliklerini yoksay
        ReferenceHandler = ReferenceHandler.IgnoreCycles,

        // Json çıktısında girintileme yap
        WriteIndented = true,

        // Özellik adları büyük/küçük harfe duyarsız
        PropertyNameCaseInsensitive = false,

        // Özellik adları camelCase formatında
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

        // JavaScript kodlaması için daha az katı bir kodlayıcı kullan
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}

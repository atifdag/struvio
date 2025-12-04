namespace Struvio.Persistence.Utils;

/// <summary>
/// IEntity arayüzünü uygulayan nesneler için JSON dönüştürücüleri oluşturan fabrika sınıfı.
/// Geçmiş kayıtları oluştururken varlıkları JSON'a dönüştürmek için kullanılır.
/// </summary>
internal class HistoryConverterFactory : JsonConverterFactory
{
    /// <summary>
    /// Belirtilen türün dönüştürülebilir olup olmadığını kontrol eder.
    /// </summary>
    /// <param name="typeToConvert">Kontrol edilecek tür</param>
    /// <returns>Tür IEntity arayüzünü uyguluyorsa true, aksi halde false</returns>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.GetInterfaces().FirstOrDefault(x => x.Name == nameof(IEntity)) != null;
    }

    /// <summary>
    /// Belirtilen tür için bir JSON dönüştürücü oluşturan bir yöntem.
    /// </summary>
    /// <param name="typeToConvert">Dönüştürülecek tür.</param>
    /// <param name="options">JSON serileştirme seçenekleri.</param>
    /// <returns>Oluşturulan JSON dönüştürücü.</returns>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return Activator.CreateInstance(typeof(HistoryConverter<>).MakeGenericType(typeToConvert)) is not JsonConverter converter
            ? throw new InvalidOperationException($"Unable to create converter for {typeToConvert}.")
            : converter;
    }
}

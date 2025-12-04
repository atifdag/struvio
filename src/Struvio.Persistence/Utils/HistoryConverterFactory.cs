namespace Struvio.Persistence.Utils;

/// <summary>
/// HistoryConverterFactory, IEntity arayüzünü uygulayan nesneler için JSON dönüştürücüleri oluşturan bir fabrika sınıfıdır.
/// </summary>
internal class HistoryConverterFactory : JsonConverterFactory
{
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

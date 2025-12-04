namespace Struvio.Persistence.Utils;

/// <summary>
/// JSON verilerini History nesnelerine dönüştüren bir JsonConverter.
/// </summary>
/// <typeparam name="T">Dönüştürülecek nesne türü.</typeparam>
internal class HistoryConverter<T> : JsonConverter<T>
{
    /// <summary>
    /// JSON verisini belirtilen türde bir nesneye dönüştürür.
    /// </summary>
    /// <param name="reader">JSON okuyucu.</param>
    /// <param name="typeToConvert">Dönüştürülecek tür.</param>
    /// <param name="options">Serileştirme seçenekleri.</param>
    /// <returns>Dönüştürülmüş nesne.</returns>
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = JsonSerializer.Deserialize<T>(ref reader, PersistenceConstants.DefaultSerializerOptions) ?? throw new JsonException($"Deserialization returned null for type {typeof(T)}.");
        return result;
    }

    /// <summary>
    /// Belirtilen nesneyi JSON formatında yazar.
    /// </summary>
    /// <param name="writer">JSON yazıcı.</param>
    /// <param name="value">Yazılacak nesne.</param>
    /// <param name="options">Serileştirme seçenekleri.</param>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        using var document = JsonDocument.Parse(JsonSerializer.Serialize(value, PersistenceConstants.DefaultSerializerOptions));
        foreach (var property in document.RootElement.EnumerateObject().Where(property => property.Value.ValueKind != JsonValueKind.Array))
        {
            if (property.Value.ValueKind != JsonValueKind.Object)
            {
                property.WriteTo(writer);
            }
            else
            {
                writer.WriteStartObject(property.Name);
                writer.WritePropertyName("Id");
                writer.WriteStringValue(property.Value.GetProperty("Id").ToString());
                writer.WriteEndObject();
            }
        }
        writer.WriteEndObject();
    }
}

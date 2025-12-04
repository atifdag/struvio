namespace Struvio.Persistence.Utils;

/// <summary>
/// JSON verilerini History nesnelerine dönüştüren bir JsonConverter.
/// </summary>
/// <typeparam name="T">Dönüştürülecek nesne türü.</typeparam>
internal class HistoryConverter<T> : JsonConverter<T>
{
    // Özellikleri ve kategorilerini önbelleğe al (Reflection maliyetini düşürür)
    private static readonly PropertyMetadata[] _properties = [.. typeof(T)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
        .Select(p => new PropertyMetadata(p, Classify(p.PropertyType)))
        .Where(p => p.Category != PropertyCategory.Collection)];

    private enum PropertyCategory { Primitive, Collection, Entity }
    private record PropertyMetadata(PropertyInfo Info, PropertyCategory Category);

    private static PropertyCategory Classify(Type type)
    {
        if (type == typeof(string) || type == typeof(byte[]))
            return PropertyCategory.Primitive;

        if (type.IsValueType)
            return PropertyCategory.Primitive;
        
        if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
            return PropertyCategory.Collection;

        return PropertyCategory.Entity;
    }

    /// <summary>
    /// JSON verisini belirtilen türde bir nesneye dönüştürür.
    /// </summary>
    /// <param name="reader">JSON okuyucu.</param>
    /// <param name="typeToConvert">Dönüştürülecek tür.</param>
    /// <param name="options">Serileştirme seçenekleri.</param>
    /// <returns>Dönüştürülmüş nesne.</returns>
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<T>(ref reader, PersistenceConstants.DefaultSerializerOptions) ?? throw new JsonException($"Deserialization returned null for type {typeof(T)}.");
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
        
        foreach (var meta in _properties)
        {
            var propVal = meta.Info.GetValue(value);
            
            if (propVal is null)
            {
                writer.WriteNull(meta.Info.Name);
                continue;
            }

            if (meta.Category == PropertyCategory.Entity)
            {
                // İlişkili entity ise sadece Id'sini yaz
                var idProp = propVal.GetType().GetProperty("Id");
                if (idProp != null)
                {
                    var idVal = idProp.GetValue(propVal);
                    writer.WriteStartObject(meta.Info.Name);
                    writer.WritePropertyName("Id");
                    JsonSerializer.Serialize(writer, idVal, options);
                    writer.WriteEndObject();
                }
            }
            else // Primitive
            {
                writer.WritePropertyName(meta.Info.Name);
                JsonSerializer.Serialize(writer, propVal, options);
            }
        }

        writer.WriteEndObject();
    }
}

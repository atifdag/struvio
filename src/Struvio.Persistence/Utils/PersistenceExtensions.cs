namespace Struvio.Persistence.Utils;

internal static class PersistenceExtensions
{
    extension(EntityEntry change)
    {
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

    internal static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = false
    };

    internal static readonly JsonSerializerOptions HistorySerializerOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = false,
        Converters = {
            new HistoryConverterFactory()
        }
    };

    internal static ValueConverter<string, string> EncryptionValueConverter = new(v => v.Encrypt(), v => v.Decrypt());

    internal static JsonDocument ToCreateHistoryAsJson(this object entity)
    {
        return JsonSerializer.SerializeToDocument(entity, HistorySerializerOptions);
    }
}

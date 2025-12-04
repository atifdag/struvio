namespace Struvio.Persistence.Utils;

internal class PersistenceConstants
{
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

    internal const int Max11Lenght = 11;
    internal const int Max50Lenght = 50;
    internal const int Max256Lenght = 256;
    internal const int Max512Lenght = 256;
    internal const int Max2000Lenght = 2000;
}

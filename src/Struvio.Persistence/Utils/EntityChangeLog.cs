namespace Struvio.Persistence.Utils;

internal record EntityChangeLog(string PropertyName, string OldValue, string NewValue);

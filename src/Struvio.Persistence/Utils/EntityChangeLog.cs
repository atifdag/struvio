namespace Struvio.Persistence.Utils;

/// <summary>
/// Varlık değişiklik kaydı. Bir özelliğin eski ve yeni değerlerini tutar.
/// </summary>
/// <param name="PropertyName">Değişen özelliğin adı</param>
/// <param name="OldValue">Özelliğin eski değeri</param>
/// <param name="NewValue">Özelliğin yeni değeri</param>
internal record EntityChangeLog(string PropertyName, string OldValue, string NewValue);

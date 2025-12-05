namespace Struvio.Common.ValueObjects;

/// <summary>
/// Sıralama için alan ve yön bilgilerini tutan bir kayıt
/// </summary>
/// <param name="Field"></param>
/// <param name="Direction"></param>
public record SortItem(string Field, string Direction);

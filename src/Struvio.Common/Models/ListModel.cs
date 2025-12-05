namespace Struvio.Common.Models;

/// <summary>
/// Listeleme işlemlerinde kullanılacak jenerik sınıf
/// </summary>
public class ListModel<T> where T : class, ICrudModel, new()
{
    /// <summary>
    /// Sayfalama bilgilerini temsil eden nesne
    /// </summary>
    public Paging? Paging { get; set; }

    /// <summary>
    /// Liste öğelerini temsil eden dizi
    /// </summary>
    public T[]? Items { get; set; }

    /// <summary>
    /// Sıralama bilgilerini temsil eden dizi
    /// </summary>
    public SortItem[]? Sorting { get; set; }
}

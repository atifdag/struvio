namespace Struvio.Common.ValueObjects;

/// <summary>
/// Sayfalama bilgilerini temsil eden sınıf.
/// </summary>
public class Paging
{
    /// <summary>
    /// Toplam sayfa sayısını alır veya ayarlar.
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    /// Toplam öğe sayısını alır veya ayarlar.
    /// </summary>
    public int TotalItemCount { get; set; }

    /// <summary>
    /// Mevcut sayfa numarasını alır veya ayarlar.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Sayfa başına öğe sayısını alır veya ayarlar.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Önceki sayfanın olup olmadığını gösterir.
    /// </summary>
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// Sonraki sayfanın olup olmadığını gösterir.
    /// </summary>
    public bool HasNextPage { get; set; }

    /// <summary>
    /// İlk sayfa olup olmadığını gösterir.
    /// </summary>
    public bool IsFirstPage { get; set; }

    /// <summary>
    /// Son sayfa olup olmadığını gösterir.
    /// </summary>
    public bool IsLastPage { get; set; }

    /// <summary>
    /// Kullanılabilir sayfa boyutlarını alır veya ayarlar.
    /// </summary>
    public int[]? PageSizes { get; set; }
}

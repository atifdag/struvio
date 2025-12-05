using Struvio.Common.Enums;

namespace Struvio.Common.Models;


/// <summary>
/// Filtre işlemlerinde kullanılacak sınıf
/// </summary>
public class FilterModel
{
    // Başlangıç tarihini temsil eden özellik
    [Display(Name = nameof(LanguageTexts.StartDate), ResourceType = typeof(LanguageTexts))]
    public DateTime StartDate { get; set; } = DateTime.UtcNow.AddYears(-2); // Varsayılan olarak 2 yıl önce (UTC)

    // Bitiş tarihini temsil eden özellik
    [Display(Name = nameof(LanguageTexts.EndDate), ResourceType = typeof(LanguageTexts))]
    public DateTime EndDate { get; set; } = DateTime.UtcNow; // Varsayılan olarak şu anki UTC tarih

    // Sayfa numarasını temsil eden özellik
    public int PageNumber { get; set; } = 1; // Varsayılan olarak 1. sayfa

    // Sayfa boyutunu temsil eden özellik
    public int PageSize { get; set; } // Sayfa boyutu belirtilmemiş

    // Durumunu temsil eden özellik
    [Display(Name = nameof(LanguageTexts.Status), ResourceType = typeof(LanguageTexts))]
    public int Status { get; set; } = (int)StatusOptions.All; // Varsayılan olarak tüm durumlar

    // Arama terimini temsil eden özellik
    [Display(Name = nameof(LanguageTexts.Searched), ResourceType = typeof(LanguageTexts))]
    public string? Searched { get; set; } // Arama terimi isteğe bağlı

    // Sıralama kriterlerini temsil eden özellik
    public SortItem[] Sorting { get; set; } = [new("SequenceNumber", SortDirection.asc.ToString())]; // Varsayılan sıralama
}

namespace Struvio.Common.Enums;

/// <summary>
/// Durum seçenekleri için enum tanımı
/// </summary>
public enum StatusOptions
{
    // Onaylı durumu
    [Display(Name = nameof(LanguageTexts.Approved), ResourceType = typeof(LanguageTexts))]
    Approved = 1,

    // Onaylı değil durumu
    [Display(Name = nameof(LanguageTexts.NotApproved), ResourceType = typeof(LanguageTexts))]
    NotApproved = 0,

    // Tüm durumlar
    [Display(Name = nameof(LanguageTexts.All), ResourceType = typeof(LanguageTexts))]
    All = -1
}

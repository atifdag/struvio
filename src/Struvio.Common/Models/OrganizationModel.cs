namespace Struvio.Common.Models;

/// <summary>
/// Organizasyon bilgilerini içeren model sınıfıdır.
/// </summary>
public class OrganizationModel : BaseCrudModel
{
    [Display(Name = nameof(LanguageTexts.Code), ResourceType = typeof(LanguageTexts))]
    public string? Code { get; set; }

    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.Organization), ResourceType = typeof(LanguageTexts))]
    public string Name { get; set; } = null!;

    [Display(Name = nameof(LanguageTexts.ApiKey), ResourceType = typeof(LanguageTexts))]
    public string? ApiKey { get; set; }

    [Display(Name = nameof(LanguageTexts.ApiPassword), ResourceType = typeof(LanguageTexts))]
    public string? ApiPassword { get; set; }

    [Display(Name = nameof(LanguageTexts.Description), ResourceType = typeof(LanguageTexts))]
    public string? Description { get; set; }

    public OrganizationUserModel[]? OrganizationUsers { get; set; }

}

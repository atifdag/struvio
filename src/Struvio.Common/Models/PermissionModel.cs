namespace Struvio.Common.Models;

/// <summary>
/// Yetkileri temsil eden model sınıfıdır.
/// </summary>
public class PermissionModel : BaseCrudModel
{
    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.Code), ResourceType = typeof(LanguageTexts))]
    public string Code { get; set; } = null!;

    [Display(Name = nameof(LanguageTexts.CodeLanguageSpecific), ResourceType = typeof(LanguageTexts))]
    public string? CodeLanguageSpecific { get; set; }

    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.Name), ResourceType = typeof(LanguageTexts))]
    public string Name { get; set; } = null!;

    [Display(Name = nameof(LanguageTexts.Description), ResourceType = typeof(LanguageTexts))]
    public string? Description { get; set; }

    [Display(Name = nameof(LanguageTexts.ControllerName), ResourceType = typeof(LanguageTexts))]
    public string? ControllerName { get; set; }

    [Display(Name = nameof(LanguageTexts.ActionName), ResourceType = typeof(LanguageTexts))]
    public string? ActionName { get; set; }

    [Display(Name = nameof(LanguageTexts.Path), ResourceType = typeof(LanguageTexts))]
    public string? Path { get; set; }

    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.Language), ResourceType = typeof(LanguageTexts))]
    public IdCodeName Language { get; set; } = null!;
}

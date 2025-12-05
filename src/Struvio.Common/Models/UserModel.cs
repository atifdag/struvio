namespace Struvio.Common.Models;

/// <summary>
/// Kullanıcı bilgilerini içeren model sınıfıdır.
/// </summary>
public class UserModel : BaseCrudModel
{
    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.IdentityNumber), ResourceType = typeof(LanguageTexts))]
    public string IdentityNumber { get; set; } = null!;

    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.Username), ResourceType = typeof(LanguageTexts))]
    public string Username { get; set; } = null!;

    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.Firstname), ResourceType = typeof(LanguageTexts))]
    public string Firstname { get; set; } = null!;

    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.Lastname), ResourceType = typeof(LanguageTexts))]
    public string Lastname { get; set; } = null!;

    /// <summary>
    /// Kullanıcının tam adını dönen hesaplanmış özellik
    /// </summary>
    public string DisplayName => $"{Firstname} {Lastname}";

    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [EmailAddress(ErrorMessageResourceName = nameof(LanguageTexts.InvalidEmail), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.Email), ResourceType = typeof(LanguageTexts))]
    public string Email { get; set; } = null!;

    [Display(Name = nameof(LanguageTexts.Role), ResourceType = typeof(LanguageTexts))]
    public IdName? Role { get; set; }

    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.PhoneNumber), ResourceType = typeof(LanguageTexts))]
    public string PhoneNumber { get; set; } = null!;

    [Display(Name = nameof(LanguageTexts.Organization), ResourceType = typeof(LanguageTexts))]
    public OrganizationModel? Organization { get; set; }

    public OrganizationRole[]? OrganizationRoles { get; set; }

    [Display(Name = nameof(LanguageTexts.Language), ResourceType = typeof(LanguageTexts))]
    public IdCodeName? Language { get; set; }
}

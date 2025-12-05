

namespace Struvio.Common;

/// <summary>
/// CRUD işlemlerinde kullanılan çoklu kiracılı modeller için arayüz
/// </summary>
public interface IOrganizationalCrudModel : ICrudModel
{

    /// Kiracı (Organization)
    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.Organization), ResourceType = typeof(LanguageTexts))]
    public IdName Organization { get; set; }

}

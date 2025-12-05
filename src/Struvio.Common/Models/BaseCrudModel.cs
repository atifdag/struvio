namespace Struvio.Common.Models;

/// <summary>
/// Tüm CRUD model sınıfları için temel sınıf.
/// Yaygın olarak kullanılan özellikleri içerir ve kod tekrarını önler.
/// </summary>
public abstract class BaseCrudModel : ICrudModel
{
    [Display(Name = nameof(LanguageTexts.Id), ResourceType = typeof(LanguageTexts))]
    public Guid? Id { get; set; }

    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.Approve), ResourceType = typeof(LanguageTexts))]
    public bool IsApproved { get; set; }

    [Display(Name = nameof(LanguageTexts.SequenceNumber), ResourceType = typeof(LanguageTexts))]
    public long SequenceNumber { get; set; }

    [Display(Name = nameof(LanguageTexts.Version), ResourceType = typeof(LanguageTexts))]
    public long Version { get; set; }

    [Display(Name = nameof(LanguageTexts.CreationTime), ResourceType = typeof(LanguageTexts))]
    public DateTime CreationTime { get; set; }

    [Display(Name = nameof(LanguageTexts.LastModificationTime), ResourceType = typeof(LanguageTexts))]
    public DateTime LastModificationTime { get; set; }

    [Display(Name = nameof(LanguageTexts.Creator), ResourceType = typeof(LanguageTexts))]
    public IdName? Creator { get; set; }

    [Display(Name = nameof(LanguageTexts.LastModifier), ResourceType = typeof(LanguageTexts))]
    public IdName? LastModifier { get; set; }
}

/// <summary>
/// Çok organizasyonlu CRUD model sınıfları için temel sınıf.
/// </summary>
public abstract class BaseMultiOrganizationalCrudModel : BaseCrudModel, IOrganizationalCrudModel
{
    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.Organization), ResourceType = typeof(LanguageTexts))]
    public IdName Organization { get; set; } = null!;
}
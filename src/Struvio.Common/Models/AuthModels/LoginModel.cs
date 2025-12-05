namespace Struvio.Common.Models.AuthModels;

public class LoginModel
{
    public LoginModel()
    {
        Username = string.Empty;
        Password = string.Empty;
    }

    public LoginModel(string username, string password)
    {
        Username = username;
        Password = password;
    }

    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Display(Name = nameof(LanguageTexts.Username), ResourceType = typeof(LanguageTexts))]
    public string Username { get; set; }

    [Required(ErrorMessageResourceName = nameof(LanguageTexts.FieldIsEmpty), ErrorMessageResourceType = typeof(LanguageTexts))]
    [Password]
    [Display(Name = nameof(LanguageTexts.Password), ResourceType = typeof(LanguageTexts))]
    public string Password { get; set; }
}

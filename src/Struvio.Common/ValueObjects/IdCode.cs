namespace Struvio.Common.ValueObjects;

/// <summary>
/// IdCode, bir Guid (kimlik) ve isteğe bağlı bir string (kod) içeren bir kayıt yapısıdır.
/// </summary>
/// <param name="Id"></param>
/// <param name="Code"></param>
public record IdCode(
    Guid Id,

    [Display(Name = nameof(LanguageTexts.Code), ResourceType = typeof(LanguageTexts))]
    string? Code
    );

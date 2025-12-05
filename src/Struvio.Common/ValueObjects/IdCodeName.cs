namespace Struvio.Common.ValueObjects;

/// <summary>
/// IdCodeName, bir Guid (kimlik), isteğe bağlı bir string (kod) ve isteğe bağlı bir string (isim) içeren bir kayıt yapısıdır.
/// </summary>
/// <param name="Id"></param>
/// <param name="Code"></param>
/// <param name="Name"></param>
public record IdCodeName(Guid Id, string? Code, string? Name);

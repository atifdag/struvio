namespace Struvio.Common.ValueObjects;

/// <summary>
/// IdName, bir Guid (kimlik) ve bir string (isim) içeren bir kayıt yapısıdır.
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
public record IdName(Guid Id, string? Name);

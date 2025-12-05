namespace Struvio.Common.Utils;

// Parola doğrulama için özel bir sınıf
public partial class PasswordAttribute : ValidationAttribute
{
    // Parolanın geçerliliğini kontrol eden metod
    public override bool IsValid(object? value)
    {
        // Değer bir string ise, parolayı al
        if (value is string password)
        {
            // Parola en az 8 en fazla 15 karakter olmalıdır!
            if (password.Length < 8 || password.Length > 15)
            {
                ErrorMessage = LanguageTexts.PasswordMustBeBetween8And15; // Hata mesajı ayarla
                return false; // Geçersiz
            }

            // Parola büyük harf içermelidir!
            if (!RegexHelper.UpperRegex().IsMatch(password))
            {
                ErrorMessage = LanguageTexts.PasswordMustContainUppercase; // Hata mesajı ayarla
                return false; // Geçersiz
            }

            // Parola küçük harf içermelidir!
            if (!RegexHelper.LowerRegex().IsMatch(password))
            {
                ErrorMessage = LanguageTexts.PasswordMustContainLowercase; // Hata mesajı ayarla
                return false; // Geçersiz
            }

            // Parola rakam içermelidir!
            if (!RegexHelper.NumberRegex().IsMatch(password))
            {
                ErrorMessage = LanguageTexts.PasswordMustContainNumber; // Hata mesajı ayarla
                return false; // Geçersiz
            }

            // Parola özel karakter içermelidir!
            if (!RegexHelper.SpecialRegex().IsMatch(password))
            {
                ErrorMessage = LanguageTexts.PasswordMustContainSpecialCharacters; // Hata mesajı ayarla
                return false; // Geçersiz
            }

            // Tüm kontrollerden geçtiyse, parola geçerlidir
            return true; // Geçerli
        }

        // Parola geçerli değilse
        ErrorMessage = LanguageTexts.InvalidPassword; // Hata mesajı ayarla
        return false; // Geçersiz
    }
}

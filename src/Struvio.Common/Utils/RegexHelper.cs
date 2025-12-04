using System.Text.RegularExpressions;

namespace Struvio.Common.Utils;

public static partial class RegexHelper
{
    /// <summary>
    /// 05 ile başlayıp toplam 11 haneli rakam olmalı
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"^05\d{9}$")]
    public static partial Regex PhoneNumberRegex();

    /// <summary>
    /// Büyük harf içermelidir
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"[A-Z]")]
    public static partial Regex UpperRegex();

    /// <summary>
    /// Küçük harf içermelidir
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"[a-z]")]
    public static partial Regex LowerRegex();

    /// <summary>
    /// Nümerik karakter içermelidir
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"[0-9]")]
    public static partial Regex NumberRegex();

    /// <summary>
    /// Özel karakter içermelidir
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"[\W_]")]
    public static partial Regex SpecialRegex();

    /// <summary>
    /// HTML temizliği için kullanılan regex
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"<[^>]+>|&nbsp;")]
    public static partial Regex CleanHtmlRegex();

    /// <summary>
    /// Boşluk karakterlerini temsil eder
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"\s+")]
    public static partial Regex WhiteSpaceRegex();

    /// <summary>
    /// SEO uyumlu karakterleri filtrelemek için
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"[^A-Za-z0-9_-]")]
    public static partial Regex SeoRegex();

    /// <summary>
    /// Nümerik olmayan karakterleri temsil eder
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"[^\d]")]
    public static partial Regex NotNumberRegex();
}

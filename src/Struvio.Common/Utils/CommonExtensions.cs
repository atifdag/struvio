namespace Struvio.Common.Utils;

public static class CommonExtensions
{
    /// <summary>
    /// String'i telefon numarası formatına çevirir.
    /// Farklı formatlardaki telefon numaralarını standart formata (0XXXXXXXXXX) dönüştürür.
    /// </summary>
    /// <param name="phoneNumber">Formatlanacak telefon numarası</param>
    /// <returns>Standart formatta telefon numarası</returns>
    public static string ToFormatPhoneNumber(this string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            return string.Empty;

        // Telefon numarasından sadece rakamları al
        phoneNumber = RegexHelper.NotNumberRegex().Replace(phoneNumber, "");

        if (string.IsNullOrEmpty(phoneNumber))
            return string.Empty;

        // ilk karakter alınıyor
        var firstChar = phoneNumber[0].ToString();

        // toplam karakter sayısı
        switch (phoneNumber.Length)
        {
            // toplam karakter sayısı 10 ama ilk karakter 0 değilse (5322222222)
            case 10 when firstChar != "0":
                // başına 0 ekleniyor (05322222222)
                return $"0{phoneNumber}";

            // toplam karakter sayısı 11 ve ilk karakter 0 ise işlem yapılmıyor (05322222222)
            case 11 when firstChar == "0":
                return phoneNumber;

            // toplam karakter sayısı 11 ama ilk karakter 0 değilse (*5322222222)
            case 11 when firstChar != "0":
                // sondan 10 karakter alınıyor ve başına 0 ekleniyor
                return $"0{phoneNumber[^10..]}";

            // toplam karakter sayısı 12 ve ilk karakter 9 ise (905322222222)
            case 12 when firstChar == "9":
                // baştan 1 karakter atılıyor (05322222222)
                return $"0{phoneNumber[1..]}";

            default:
                // toplam karakter sayısı 12den fazla ise (0905322222222)
                if (phoneNumber.Length > 12)
                {
                    // sondan 10 karakter alınıyor ve başına 0 ekleniyor
                    return $"0{phoneNumber[^10..]}";
                }
                break;
        }

        return phoneNumber;
    }

}

namespace Struvio.Domain.Utils;

/// <summary>
/// Domain katmanı için genişletme metodlarını içerir. Şifreleme ve şifre çözme işlemleri sağlar.
/// </summary>
public static class DomainExtensions
{
    // Sabit bir AES Key (32 karakter olmalı)
    private static readonly string AesKey = "369c2c68012a46bda70056336c6fd247";

    extension(string plainText)
    {
        /// <summary>
        /// Düz metni AES ile şifreler.
        /// </summary>
        /// <param name="plainText">Şifrelenecek düz metin</param>
        /// <returns>Şifrelenmiş metin (Base64)</returns>
        public string Encrypt()
        {
            // AES nesnesi oluştur
            using var aes = CreateAes();

            // IV (Initialization Vector) oluştur
            aes.GenerateIV();

            // Şifreleme işlemi için bir şifreleyici oluştur
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            // Şifrelenmiş veriyi yazmak için bir bellek akışı oluştur
            using var msEncrypt = new MemoryStream();

            // IV'yi bellek akışına yaz
            msEncrypt.Write(aes.IV, 0, aes.IV.Length);

            // Şifreleme akışı oluştur
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

            // Düz metni şifreleme akışına yaz
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            // Şifrelenmiş veriyi Base64 formatında döndür
            return Convert.ToBase64String(msEncrypt.ToArray());
        }
    }

    extension(string encryptedText)
    {
        /// <summary>
        /// Şifrelenmiş metni çözer.
        /// </summary>
        /// <param name="encryptedText">Şifrelenmiş metin (Base64)</param>
        /// <returns>Düz metin</returns>
        public string Decrypt()
        {
            // Şifrelenmiş metni bayt dizisine dönüştür
            byte[] cipherBytes = Convert.FromBase64String(encryptedText);

            // AES nesnesi oluştur
            using var aes = CreateAes();

            // IV'yi bayt dizisinden çıkar
            var iv = new byte[16];
            Array.Copy(cipherBytes, 0, iv, 0, iv.Length);

            // AES nesnesine IV'yi ata
            aes.IV = iv;

            // Şifre çözme işlemi için bir şifre çözücü oluştur
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            // Şifrelenmiş veriyi okumak için bir bellek akışı oluştur
            using var msDecrypt = new MemoryStream(cipherBytes, iv.Length, cipherBytes.Length - iv.Length);

            // Şifre çözme akışı oluştur
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

            // Şifrelenmiş metni düz metne dönüştür
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }
    }

    /// <summary>
    /// AES nesnesi oluşturur (anahtar ve IV yönetimi için kullanılır).
    /// </summary>
    private static Aes CreateAes()
    {
        // AES nesnesi oluştur
        var aes = Aes.Create();

        // Anahtarı bayt dizisine dönüştür
        var key = Encoding.UTF8.GetBytes(AesKey);

        // Anahtar boyutunu 32 byte'a ayarla
        Array.Resize(ref key, 32);

        // AES nesnesine anahtarı ata
        aes.Key = key;

        return aes;
    }

}

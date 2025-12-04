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
        /// Düz metni AES ile şifreler (Senkron).
        /// EF Core Value Converters gibi senkron işlemler için kullanılır.
        /// Uygulama katmanında EncryptAsync metodunu kullanın.
        /// </summary>
        /// <param name="plainText">Şifrelenecek düz metin</param>
        /// <returns>Şifrelenmiş metin (Base64)</returns>
        public string Encrypt()
        {
            // Async metodu senkron çağır (sadece EF Core için)
            return plainText.EncryptAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Düz metni AES ile asenkron olarak şifreler.
        /// Tercih edilen metod - tüm uygulama katmanında bunu kullanın.
        /// </summary>
        /// <param name="plainText">Şifrelenecek düz metin</param>
        /// <param name="cancellationToken">İptal belirteci</param>
        /// <returns>Şifrelenmiş metin (Base64)</returns>
        public async Task<string> EncryptAsync(CancellationToken cancellationToken = default)
        {
            // AES nesnesi oluştur
            using var aes = CreateAes();

            // IV (Initialization Vector) oluştur
            aes.GenerateIV();

            // Şifreleme işlemi için bir şifreleyici oluştur
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            // Şifrelenmiş veriyi yazmak için bir bellek akışı oluştur
            using var msEncrypt = new MemoryStream();

            // IV'yi bellek akışına asenkron yaz
            await msEncrypt.WriteAsync(aes.IV.AsMemory(0, aes.IV.Length), cancellationToken);

            // Şifreleme akışı oluştur
            await using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

            // Düz metni şifreleme akışına asenkron yaz
            await using (var swEncrypt = new StreamWriter(csEncrypt, leaveOpen: false))
            {
                await swEncrypt.WriteAsync(plainText.AsMemory(), cancellationToken);
            }

            // Şifrelenmiş veriyi Base64 formatında döndür
            return Convert.ToBase64String(msEncrypt.ToArray());
        }
    }

    extension(string encryptedText)
    {
        /// <summary>
        /// Şifrelenmiş metni çözer (Senkron).
        /// EF Core Value Converters gibi senkron işlemler için kullanılır.
        /// Uygulama katmanında DecryptAsync metodunu kullanın.
        /// </summary>
        /// <param name="encryptedText">Şifrelenmiş metin (Base64)</param>
        /// <returns>Düz metin</returns>
        public string Decrypt()
        {
            // Async metodu senkron çağır (sadece EF Core için)
            return encryptedText.DecryptAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Şifrelenmiş metni asenkron olarak çözer.
        /// Tercih edilen metod - tüm uygulama katmanında bunu kullanın.
        /// </summary>
        /// <param name="encryptedText">Şifrelenmiş metin (Base64)</param>
        /// <param name="cancellationToken">İptal belirteci</param>
        /// <returns>Düz metin</returns>
        public async Task<string> DecryptAsync(CancellationToken cancellationToken = default)
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
            await using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

            // Şifrelenmiş metni düz metne asenkron dönüştür
            using var srDecrypt = new StreamReader(csDecrypt);

            return await srDecrypt.ReadToEndAsync(cancellationToken);
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

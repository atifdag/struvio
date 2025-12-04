namespace Struvio.Domain.Utils;

/// <summary>
/// Domain katmanı için genişletme metodlarını içerir. Şifreleme ve şifre çözme işlemleri sağlar.
/// </summary>
public static class DomainExtensions
{
    // Sabit bir AES Key (32 karakter olmalı)
    private static readonly string AesKey = "369c2c68012a46bda70056336c6fd247";

    extension(Stream stream)
    {
        /// <summary>
        /// Stream'den asenkron olarak nesneyi deserialize eder
        /// </summary>
        /// <typeparam name="T">Dönüştürülecek nesne tipi</typeparam>
        /// <param name="stream">Veri akışı</param>
        /// <param name="token">İptal belirteci</param>
        /// <returns>Deseralize edilmiş nesne</returns>
        public async Task<T?> DeserializeFromStringAsync<T>(CancellationToken token)
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, JsonConstants.DefaultSerializerOptions, token);
        }
    }

    extension(object obj)
    {
        /// <summary>
        /// Nesneyi string olarak döner (Senkron).
        /// EF Core gibi senkron işlemler için kullanılır.
        /// Uygulama katmanında SerializeAsync metodunu kullanın.
        /// </summary>
        /// <param name="obj">Dönüştürülecek nesne</param>
        /// <returns>String olarak dönen nesne</returns>
        public string Serialize() => JsonSerializer.Serialize(obj, JsonConstants.DefaultSerializerOptions);

        /// <summary>
        /// Nesneyi asenkron olarak string'e serialize eder.
        /// Tercih edilen metod - büyük nesneler için thread pool kullanır.
        /// </summary>
        /// <param name="obj">Dönüştürülecek nesne</param>
        /// <param name="cancellationToken">İptal belirteci</param>
        /// <returns>Serialize edilmiş JSON string</returns>
        public async Task<string> SerializeAsync(CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, obj, obj.GetType(), JsonConstants.DefaultSerializerOptions, cancellationToken);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync(cancellationToken);
        }
    }

    extension(string json)
    {
        /// <summary>
        /// String bir değeri nesneye çevirir (Senkron).
        /// EF Core gibi senkron işlemler için kullanılır.
        /// Uygulama katmanında DeserializeAsync metodunu kullanın.
        /// </summary>
        /// <typeparam name="T">Dönüştürülecek nesne tipi</typeparam>
        /// <param name="json">Dönüştürülecek JSON stringi</param>
        /// <returns>Deseralize edilmiş nesne</returns>
        public T? Deserialize<T>() => JsonSerializer.Deserialize<T>(json, JsonConstants.DefaultSerializerOptions);

        /// <summary>
        /// String bir değeri asenkron olarak nesneye çevirir.
        /// Tercih edilen metod - büyük JSON'lar için thread pool kullanır.
        /// </summary>
        /// <typeparam name="T">Dönüştürülecek nesne tipi</typeparam>
        /// <param name="json">Dönüştürülecek JSON stringi</param>
        /// <param name="cancellationToken">İptal belirteci</param>
        /// <returns>Deserialize edilmiş nesne</returns>
        public async Task<T?> DeserializeAsync<T>(CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return await JsonSerializer.DeserializeAsync<T>(stream, JsonConstants.DefaultSerializerOptions, cancellationToken);
        }
    }

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

    extension(Stream inputStream)
    {
        /// <summary>
        /// Stream'i asenkron olarak şifreler (stream-to-stream AES encryption).
        /// Büyük dosyalar ve cache işlemleri için optimize edilmiştir.
        /// </summary>
        /// <param name="inputStream">Şifrelenecek input stream</param>
        /// <param name="outputStream">Şifrelenmiş veriyi yazacak output stream</param>
        /// <param name="cancellationToken">İptal belirteci</param>
        public async Task EncryptToStreamAsync(Stream outputStream, CancellationToken cancellationToken = default)
        {
            using var aes = CreateAes();
            aes.GenerateIV();

            // IV'yi output stream'e yaz
            await outputStream.WriteAsync(aes.IV.AsMemory(0, aes.IV.Length), cancellationToken);

            // Encrypt pipeline: input → CryptoStream → output
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            await using var cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write, leaveOpen: true);
            await inputStream.CopyToAsync(cryptoStream, cancellationToken);
            await cryptoStream.FlushFinalBlockAsync(cancellationToken);
        }

        /// <summary>
        /// Şifrelenmiş stream'i asenkron olarak çözer (stream-to-stream AES decryption).
        /// Büyük dosyalar ve cache işlemleri için optimize edilmiştir.
        /// </summary>
        /// <param name="inputStream">Şifrelenmiş input stream</param>
        /// <param name="outputStream">Çözülmüş veriyi yazacak output stream</param>
        /// <param name="cancellationToken">İptal belirteci</param>
        public async Task DecryptFromStreamAsync(Stream outputStream, CancellationToken cancellationToken = default)
        {
            using var aes = CreateAes();

            // IV'yi oku (ilk 16 byte)
            var iv = new byte[16];
            await inputStream.ReadExactlyAsync(iv.AsMemory(0, iv.Length), cancellationToken);
            aes.IV = iv;

            // Decrypt pipeline: input → CryptoStream → output
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            await using var cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read, leaveOpen: true);
            await cryptoStream.CopyToAsync(outputStream, cancellationToken);
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

namespace Struvio.Domain;

public interface ICacheSettings
{
    /// <summary>
    /// Redis Sentinel kullanılıyorsa true
    /// </summary>
    bool UseSentinel { get; set; }

    /// <summary>
    /// Sentinel master adı
    /// </summary>
    string? ServiceName { get; set; }

    /// <summary>
    /// Sentinel sunucu adresleri (host:port)
    /// </summary>
    string[]? Sentinels { get; set; }

    /// <summary>
    /// Bağlantı bilgisi
    /// </summary>
    string[] EndPoints { get; set; }

    /// <summary>
    /// Bu süre boyunca istenmezse, önbelleğe alınmış bir nesne silinir
    /// </summary>
    double SlidingExpirationSecond { get; set; }

    /// <summary>
    /// önbelleğe alınmış nesnenin sona erme süresi
    /// </summary>
    double AbsoluteExpirationSecond { get; set; }

    /// <summary>
    /// Ana anahtar
    /// </summary>
    public string MainKey { get; set; }

    /// <summary>
    /// Redis kullanılacak mı?
    /// </summary>
    bool UseRedisCache { get; set; }

    /// <summary>
    /// Bağlantı zaman aşımı
    /// </summary>
    int ConnectTimeout { get; set; }

    /// <summary>
    ///  senkron (eşzamanlı) işlemlerin maksimum bekleme süresi
    /// </summary>
    int SyncTimeout { get; set; }

    /// <summary>
    /// bağlanma girişimi başarısız olduğunda yapılacak yeniden deneme sayısı
    /// </summary>
    int ConnectRetry { get; set; }

    /// <summary>
    /// Sunucu ile istemci arasındaki bağlantının canlı tutulması için gönderilen periyodik sinyallerin sıklığını belirler (saniye cinsinden)
    /// </summary>
    int KeepAlive { get; set; }

    /// <summary>
    /// Varsayılan veritabanı
    /// </summary>
    int? DefaultDatabase { get; set; }

    /// <summary>
    /// Parola
    /// </summary>
    string? Password { get; set; }
}

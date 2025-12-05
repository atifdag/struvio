namespace Struvio.Application.Caching;

public sealed class CacheSettings : ICacheSettings
{
    public bool UseSentinel { get; set; }
    public string? ServiceName { get; set; }
    public string[]? Sentinels { get; set; }
    public string[] EndPoints { get; set; } = [];
    public double SlidingExpirationSecond { get; set; } = 1200; // 20 dakika
    public double AbsoluteExpirationSecond { get; set; } = 3600; // 60 dakika
    public string MainKey { get; set; } = "Struvio";
    public bool UseRedisCache { get; set; }
    public int ConnectTimeout { get; set; } = 5000;
    public int SyncTimeout { get; set; } = 5000;
    public int ConnectRetry { get; set; } = 3;
    public int KeepAlive { get; set; } = 60;
    public int? DefaultDatabase { get; set; }
    public string? Password { get; set; }
}

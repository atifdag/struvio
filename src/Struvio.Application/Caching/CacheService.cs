using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Struvio.Application.Caching;

/// <summary>
/// Modern HybridCache implementasyonu (.NET 9+ standartlarına uygun).
/// L1 (IMemoryCache) + L2 (Redis) katmanlı cache mimarisi.
/// 
/// Özellikler:
/// - Stampede Protection: Per-key locking ile eş zamanlı DB çağrılarını engeller
/// - Stream-Based: Zero-copy pipeline ile düşük bellek kullanımı
/// - Tag-Based Invalidation: Gruplu cache temizleme
/// - Security: AES-256 encryption ile veri güvenliği
/// </summary>
public sealed class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;
    private readonly ICacheSettings _cacheSettings;
    private readonly IStruvioLogger _logger;
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _keyLocks = new();
    private readonly ConcurrentDictionary<string, HashSet<string>> _tagIndex = new();

    public CacheService(
        IMemoryCache memoryCache,
        IDistributedCache distributedCache,
        ICacheSettings cacheSettings,
        IStruvioLogger logger)
    {
        ArgumentNullException.ThrowIfNull(memoryCache);
        ArgumentNullException.ThrowIfNull(distributedCache);
        ArgumentNullException.ThrowIfNull(cacheSettings);
        ArgumentNullException.ThrowIfNull(logger);

        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
        _cacheSettings = cacheSettings;
        _logger = logger;

        if (_cacheSettings.AbsoluteExpirationSecond <= 0)
            throw new ArgumentException("AbsoluteExpirationSecond must be > 0", nameof(cacheSettings));
        if (_cacheSettings.SlidingExpirationSecond <= 0)
            throw new ArgumentException("SlidingExpirationSecond must be > 0", nameof(cacheSettings));
    }

    /// <summary>
    /// L2 (distributed) cache options oluşturur
    /// </summary>
    private DistributedCacheEntryOptions CreateDistributedOptions(TimeSpan? expiration = null) => new()
    {
        AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromSeconds(_cacheSettings.AbsoluteExpirationSecond),
        SlidingExpiration = TimeSpan.FromSeconds(_cacheSettings.SlidingExpirationSecond)
    };

    /// <summary>
    /// L1 (memory) cache options oluşturur
    /// </summary>
    private MemoryCacheEntryOptions CreateMemoryOptions(TimeSpan? expiration = null) => new()
    {
        AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromSeconds(Math.Min(_cacheSettings.AbsoluteExpirationSecond, 300)), // L1 max 5 min
        SlidingExpiration = TimeSpan.FromSeconds(Math.Min(_cacheSettings.SlidingExpirationSecond, 60)),
        Size = 1 // Memory cache size tracking için
    };

    /// <summary>
    /// Key-specific lock alır (stampede protection)
    /// </summary>
    private SemaphoreSlim GetKeyLock(string key) =>
        _keyLocks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

    /// <summary>
    /// Redis bağlantı sağlık kontrolü yapar.
    /// </summary>
    public async Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            const string pingKey = "health:ping";
            await _distributedCache.SetStringAsync(pingKey, "pong", CreateDistributedOptions(TimeSpan.FromSeconds(10)), cancellationToken);
            var result = await _distributedCache.GetStringAsync(pingKey, cancellationToken);
            await _distributedCache.RemoveAsync(pingKey, cancellationToken);
            
            var isConnected = result == "pong";
            _logger.Information("L2 cache health: {Status}", isConnected ? "Healthy" : "Unhealthy");
            return isConnected;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "L2 cache health check failed");
            return false;
        }
    }

    /// <summary>
    /// L1 (Memory) ve L2 (Redis) cache'e değer yazar.
    /// Stream-based pipeline: Serialize → Encrypt → Redis.
    /// </summary>
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        if (value is null) return;

        try
        {
            // L1 (Memory) write - direkt nesne
            _memoryCache.Set(key, value, CreateMemoryOptions(expiration));

            // L2 (Distributed) write - full stream pipeline
            await using var serializedStream = new MemoryStream();
            
            // 1. Serialize to stream
            await JsonSerializer.SerializeAsync(serializedStream, value, value.GetType(), JsonConstants.DefaultSerializerOptions, cancellationToken);
            serializedStream.Position = 0;

            // 2. Encrypt stream to stream (DomainExtensions)
            await using var encryptedStream = new MemoryStream();
            await serializedStream.EncryptToStreamAsync(encryptedStream, cancellationToken);
            
            // 3. Write to distributed cache
            var bytes = encryptedStream.ToArray();
            await _distributedCache.SetAsync(key, bytes, CreateDistributedOptions(expiration), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to set cache key: {Key}", key);
            throw;
        }
    }

    /// <summary>
    /// L1 → L2 cascade read yapar. L1 miss ise L2'den okur ve L1'e backfill yapar.
    /// </summary>
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        try
        {
            // L1 (Memory) check - fast path
            if (_memoryCache.TryGetValue<T>(key, out var memoryValue))
            {
                return memoryValue;
            }

            // L2 (Distributed) check
            var encryptedBytes = await _distributedCache.GetAsync(key, cancellationToken);
            if (encryptedBytes == null || encryptedBytes.Length == 0)
                return default;

            // Full stream pipeline: decrypt → deserialize
            await using var encryptedStream = new MemoryStream(encryptedBytes);
            await using var decryptedStream = new MemoryStream();
            
            // 1. Decrypt stream to stream (DomainExtensions)
            await encryptedStream.DecryptFromStreamAsync(decryptedStream, cancellationToken);
            decryptedStream.Position = 0;

            // 2. Deserialize from stream
            var value = await JsonSerializer.DeserializeAsync<T>(decryptedStream, JsonConstants.DefaultSerializerOptions, cancellationToken);

            // Backfill L1 from L2
            if (value is not null)
            {
                _memoryCache.Set(key, value, CreateMemoryOptions());
            }

            return value;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to get cache key: {Key}", key);
            return default;
        }
    }

    /// <summary>
    /// Cache-Aside pattern: Cache'de varsa döner, yoksa factory çalıştırıp cache'e yazar.
    /// Stampede protection ile aynı key için tek factory çalışır.
    /// Tag-based invalidation için opsiyonel etiket desteği.
    /// </summary>
    public async Task<T> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? expiration = null, string[]? tags = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(factory);

        // Fast path: L1 check (no lock)
        var cached = await GetAsync<T>(key, cancellationToken);
        if (cached is not null && !EqualityComparer<T>.Default.Equals(cached, default))
            return cached;

        // Per-key lock (stampede protection)
        var keyLock = GetKeyLock(key);
        await keyLock.WaitAsync(cancellationToken);
        try
        {
            // Double-check pattern
            cached = await GetAsync<T>(key, cancellationToken);
            if (cached is not null && !EqualityComparer<T>.Default.Equals(cached, default))
                return cached;

            // Factory execution
            var value = await factory(cancellationToken);
            if (value is not null)
            {
                await SetAsync(key, value, expiration, cancellationToken);

                // Tag indexing için kaydet
                if (tags is not null && tags.Length > 0)
                {
                    foreach (var tag in tags)
                    {
                        _tagIndex.AddOrUpdate(tag,
                            _ => [key],
                            (_, set) => { set.Add(key); return set; });
                    }
                }
            }
            return value;
        }
        finally
        {
            keyLock.Release();
        }
    }

    /// <summary>
    /// Belirtilen anahtarı L1 ve L2 cache'den siler.
    /// </summary>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        try
        {
            _memoryCache.Remove(key);
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to remove cache key: {Key}", key);
            throw;
        }
    }

    /// <summary>
    /// Birden fazla cache anahtarını paralel olarak siler.
    /// </summary>
    public async Task RemoveManyAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(keys);

        var keyList = keys.ToList();
        if (keyList.Count == 0) return;

        try
        {
            // L1 removal (sync)
            foreach (var key in keyList)
                _memoryCache.Remove(key);

            // L2 removal (parallel)
            var tasks = keyList.Select(key => _distributedCache.RemoveAsync(key, cancellationToken));
            await Task.WhenAll(tasks);
            
            _logger.Information("Removed {Count} cache keys", keyList.Count);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to remove multiple cache keys");
            throw;
        }
    }

    /// <summary>
    /// Belirtilen tag'e sahip tüm cache'leri siler.
    /// Tag indexi in-memory tutulur, Redis restart sonrası kaybolur.
    /// </summary>
    public async Task RemoveByTagAsync(string tag, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);

        try
        {
            if (_tagIndex.TryRemove(tag, out var keys))
            {
                await RemoveManyAsync(keys, cancellationToken);
                _logger.Information("Invalidated {Count} cache entries for tag: {Tag}", keys.Count, tag);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to remove cache by tag: {Tag}", tag);
            throw;
        }
    }

}

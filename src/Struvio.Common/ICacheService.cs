namespace Struvio.Common;

/// <summary>
/// Modern hybrid cache servis arayüzü (.NET 9+ HybridCache tabanlı).
/// L1 (in-memory) + L2 (distributed Redis) katmanlı önbellekleme.
/// Tüm metodlar async-only, strongly-typed ve cancellation-aware.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Önbellek bağlantısını kontrol eder (L2 distributed cache).
    /// </summary>
    Task<bool> CanConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Önbelleğe değer yazar (L1 + L2).
    /// </summary>
    /// <param name="key">Cache anahtarı</param>
    /// <param name="value">Cache'lenecek değer</param>
    /// <param name="expiration">Opsiyonel expiration süresi (null ise default settings kullanılır)</param>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Önbellekten değer okur. Bulunamazsa null döner.
    /// L1 (memory) -> L2 (redis) sırasıyla kontrol edilir.
    /// </summary>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cache-aside pattern: Önbellekte yoksa factory ile oluşturur, yazar ve döner.
    /// Stampede protection ile aynı key için paralel factory çağrıları önlenir.
    /// </summary>
    /// <param name="key">Cache anahtarı</param>
    /// <param name="factory">Değer üreteci (async)</param>
    /// <param name="expiration">Opsiyonel expiration süresi</param>
    /// <param name="tags">Cache invalidation için tag'ler</param>
    Task<T> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? expiration = null, string[]? tags = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Önbellekten anahtarı siler (L1 + L2).
    /// </summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Birden fazla anahtarı toplu olarak siler.
    /// </summary>
    Task RemoveManyAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tag'e göre cache'leri invalidate eder.
    /// Örnek: RemoveByTagAsync("user:123") tüm "user:123" tag'li cache'leri temizler.
    /// </summary>
    Task RemoveByTagAsync(string tag, CancellationToken cancellationToken = default);
}

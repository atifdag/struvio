using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Struvio.UI.Web.Api.Utils;

/// <summary>
/// Health Check yapılandırma extension methods.
/// Kubernetes-compatible health check endpoints ile tüm bağımlılıkları izler.
/// </summary>
public static class HealthCheckExtensions
{
    /// <summary>
    /// Tüm health check servislerini ekler.
    /// Database, Cache, Memory ve API kontrollerini içerir.
    /// </summary>
    public static IServiceCollection AddStruvioHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            // Database Health Check
            .AddDbContextCheck<ApplicationDbContext>(
                name: "database",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["db", "sql", "sqlite", "ready"])
            
            // Memory Cache Health Check
            .AddCheck("memory-cache", () =>
            {
                try
                {
                    var memoryCache = services.BuildServiceProvider().GetService<Microsoft.Extensions.Caching.Memory.IMemoryCache>();
                    return memoryCache != null
                        ? HealthCheckResult.Healthy("Memory cache is available")
                        : HealthCheckResult.Degraded("Memory cache service not found");
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy("Memory cache check failed", ex);
                }
            }, tags: ["cache", "memory", "ready"])
            
            // Distributed Cache (Redis) Health Check
            .AddCheck("distributed-cache", () =>
            {
                try
                {
                    var cacheSettings = services.BuildServiceProvider().GetService<ICacheSettings>();
                    if (cacheSettings?.UseRedisCache == true)
                    {
                        var distributedCache = services.BuildServiceProvider().GetService<IDistributedCache>();
                        if (distributedCache == null)
                            return HealthCheckResult.Unhealthy("Distributed cache service not found");

                        // Simple connectivity test
                        var testKey = $"health-check-{Guid.NewGuid()}";
                        distributedCache.SetString(testKey, "test", new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
                        });
                        var value = distributedCache.GetString(testKey);
                        distributedCache.Remove(testKey);

                        return value == "test"
                            ? HealthCheckResult.Healthy("Distributed cache (Redis) is working")
                            : HealthCheckResult.Degraded("Distributed cache read/write test failed");
                    }
                    
                    return HealthCheckResult.Healthy("Distributed cache not enabled");
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Degraded("Distributed cache check failed (Redis might be down, but app can work with memory cache)", ex);
                }
            }, tags: ["cache", "redis", "distributed", "ready"])
            
            // API Self Check
            .AddCheck("api", () =>
                HealthCheckResult.Healthy("API is running"),
                tags: ["api", "self", "live"]);

        return services;
    }

    /// <summary>
    /// Health check endpoint'ini map eder.
    /// /health - Database, Cache, Memory ve API durumlarını detaylı gösterir
    /// </summary>
    public static IEndpointRouteBuilder MapStruvioHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var result = System.Text.Json.JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    timestamp = DateTime.UtcNow,
                    totalDuration = $"{report.TotalDuration.TotalMilliseconds:F2}ms",
                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description,
                        duration = $"{entry.Value.Duration.TotalMilliseconds:F2}ms",
                        tags = entry.Value.Tags,
                        exception = entry.Value.Exception?.Message,
                        data = entry.Value.Data.Count > 0 ? entry.Value.Data : null
                    }).OrderBy(x => x.name)
                }, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await context.Response.WriteAsync(result);
            }
        });

        return endpoints;
    }
}

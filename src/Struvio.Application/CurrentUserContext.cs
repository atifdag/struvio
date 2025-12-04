using Microsoft.EntityFrameworkCore;

namespace Struvio.Application;

/// <summary>
/// Mevcut kullanıcının kimlik doğrulama bağlamını yönetir.
/// ClaimsPrincipal'dan kullanıcı ID, dil ve organizasyon bilgilerine erişim sağlar.
/// Scoped lifetime ile çalışır, dil ve organizasyon bilgileri HybridCache ile yönetilir.
/// </summary>
public sealed class CurrentUserContext(
    ApplicationDbContext dbContext,
    ICacheService cacheService,
    IPrincipal principal,
    IStruvioLogger logger
    ) : ICurrentUserContext
{
    private readonly ClaimsPrincipal _principal = (ClaimsPrincipal)principal;
    private Guid? _cachedUserId;

    /// <summary>
    /// Oturumdaki kullanıcının ID'sini getirir.
    /// İlk çağrıda ClaimsPrincipal'dan parse edilir, sonrası bellekten döner.
    /// </summary>
    public Task<Guid> GetUserIdAsync(CancellationToken cancellationToken = default)
    {
        // Cache'lenmiş değer varsa direkt dön
        if (_cachedUserId.HasValue)
        {
            return Task.FromResult(_cachedUserId.Value);
        }

        // İlk çağrıda parse ve cache
        var userId = ParseUserIdFromClaims();
        _cachedUserId = userId;
        return Task.FromResult(userId);
    }

    /// <summary>
    /// ClaimsPrincipal'dan kullanıcı ID'sini parse eder.
    /// </summary>
    private Guid ParseUserIdFromClaims()
    {
        if (_principal.Claims == null || !_principal.Claims.Any())
        {
            logger.Error("ClaimsPrincipal'da hiç claim bulunamadı");
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }

        var claimNameIdentifier = _principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        if (claimNameIdentifier == null)
        {
            logger.Error("ClaimNameIdentifier bulunamadı. Mevcut claims: {Claims}", 
                string.Join(", ", _principal.Claims.Select(x => x.Type)));
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }

        var value = claimNameIdentifier.Value;
        if (string.IsNullOrWhiteSpace(value))
        {
            logger.Error("ClaimNameIdentifier değeri boş");
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }

        if (!Guid.TryParse(value, out var userId) || userId == Guid.Empty)
        {
            logger.Error("ClaimNameIdentifier geçersiz Guid formatında: {Value}", value);
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }

        return userId;
    }

    /// <summary>
    /// Oturumdaki kullanıcının dil ID'sini getirir.
    /// Cache'den okur, yoksa DB'den çeker (30 dakika expiration).
    /// </summary>
    public async Task<Guid> GetLanguageIdAsync(CancellationToken cancellationToken = default)
    {
        var userId = await GetUserIdAsync(cancellationToken);

        var languageId = await cacheService.GetOrSetAsync(
            $"{CacheConstants.DefaultUserLanguage}_{userId}",
            async ct =>
            {
                var id = await dbContext.Set<ApplicationUser>()
                    .Where(x => x.Id == userId)
                    .Select(t => t.LanguageId)
                    .FirstOrDefaultAsync(ct);

                if (id == Guid.Empty)
                {
                    logger.Warning("Kullanıcı {UserId} için dil bulunamadı", userId);
                }

                return id;
            },
            expiration: TimeSpan.FromMinutes(30),
            tags: ["user_language", $"user_{userId}"],
            cancellationToken);

        if (languageId == Guid.Empty)
        {
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }

        return languageId;
    }

    /// <summary>
    /// Oturumdaki kullanıcının organizasyon ID'sini getirir.
    /// Cache'den okur, yoksa DB'den çeker (30 dakika expiration).
    /// </summary>
    public async Task<Guid> GetOrganizationIdAsync(CancellationToken cancellationToken = default)
    {
        var userId = await GetUserIdAsync(cancellationToken);

        var organizationId = await cacheService.GetOrSetAsync(
            $"{CacheConstants.DefaultUserOrganization}_{userId}",
            async ct =>
            {
                var id = await dbContext.Set<ApplicationUser>()
                    .Where(x => x.Id == userId)
                    .Select(t => t.OrganizationId)
                    .FirstOrDefaultAsync(ct);

                if (id == Guid.Empty)
                {
                    logger.Warning("Kullanıcı {UserId} için organizasyon bulunamadı", userId);
                }

                return id;
            },
            expiration: TimeSpan.FromMinutes(30),
            tags: ["user_organization", $"user_{userId}"],
            cancellationToken);

        if (organizationId == Guid.Empty)
        {
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }

        return organizationId;
    }
}

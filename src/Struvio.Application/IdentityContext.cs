using Microsoft.EntityFrameworkCore;

namespace Struvio.Application;

public class IdentityContext(
    ApplicationDbContext dbContext,
    ICacheService cacheService,
    IPrincipal principal,
    IStruvioLogger logger
    ) : IIdentityContext
{
    private readonly ClaimsPrincipal _principal = (ClaimsPrincipal)principal;

    public async Task<Guid> GetUserIdAsync(CancellationToken cancellationToken = default)
    {
        if (_principal.Claims == null)
        {
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }

        var claimNameIdentifier = _principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        if (claimNameIdentifier == null)
        {
            logger.Error("ClaimNameIdentifier bulunamadı. Claims: {Claims}", [.. _principal.Claims.Select(x => x.Type)]);
            return Guid.Empty;
        }

        var value = claimNameIdentifier.Value;
        if (string.IsNullOrEmpty(value))
        {
            logger.Error("ClaimNameIdentifier değeri boş. Claims: {Claims}", [.. _principal.Claims.Select(x => x.Type)]);
            return Guid.Empty;
        }

        if (!Guid.TryParse(value, out var userId))
        {
            logger.Error("ClaimNameIdentifier değeri Guid'e dönüştürülemedi. Claims: {Claims}", [.. _principal.Claims.Select(x => x.Type)]);
            return Guid.Empty;
        }
        if (userId == default || userId == Guid.Empty)
        {
            logger.Error("ClaimNameIdentifier değeri default veya boş. Claims: {Claims}", [.. _principal.Claims.Select(x => x.Type)]);
            return Guid.Empty;
        }

        return userId;
    }
    public async Task<Guid> GetLanguageIdAsync(CancellationToken cancellationToken = default)
    {
        Guid userId = await GetUserIdAsync(cancellationToken);

        if (userId == default || userId == Guid.Empty)
        {
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }
        return await cacheService.GetAsync($"{CacheConstants.DefaultUserLanguage}_{userId}", async () => await dbContext.Set<ApplicationUser>().Where(x => x.Id == userId).Select(t => t.LanguageId).FirstOrDefaultAsync(), cancellationToken);
    }

    public async Task<Guid> GetOrganizationIdAsync(CancellationToken cancellationToken = default)
    {
        Guid userId = await GetUserIdAsync(cancellationToken);

        if (userId == default || userId == Guid.Empty)
        {
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }
        return await cacheService.GetAsync($"{CacheConstants.DefaultUserOrganization}_{userId}", async () => await dbContext.Set<ApplicationUser>().Where(x => x.Id == userId).Select(t => t.OrganizationId).FirstOrDefaultAsync(), cancellationToken);
    }

}

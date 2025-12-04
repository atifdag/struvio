namespace Struvio.Application;

public class IdentityContext(
    ApplicationDbContext dbContext,
    ICacheService cacheService,
    IPrincipal principal,
    IStruvioLogger logger
    ) : IIdentityContext
{
    private readonly ClaimsPrincipal _principal = (ClaimsPrincipal)principal;

    public Guid GetUserId()
    {
        if (_principal.Claims == null)
        {
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }

        var claimNameIdentifier = _principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        if (claimNameIdentifier == null)
        {
            logger.Error("ClaimNameIdentifier bulunamadı. Claims: {Claims}", _principal.Claims.Select(x => x.Type).ToList());
            return Guid.Empty;
        }

        var value = claimNameIdentifier.Value;
        if (string.IsNullOrEmpty(value))
        {
            logger.Error("ClaimNameIdentifier değeri boş. Claims: {Claims}", _principal.Claims.Select(x => x.Type).ToList());
            return Guid.Empty;
        }

        if (!Guid.TryParse(value, out var userId))
        {
            logger.Error("ClaimNameIdentifier değeri Guid'e dönüştürülemedi. Claims: {Claims}", _principal.Claims.Select(x => x.Type).ToList());
            return Guid.Empty;
        }
        if (userId == default || userId == Guid.Empty)
        {
            logger.Error("ClaimNameIdentifier değeri default veya boş. Claims: {Claims}", _principal.Claims.Select(x => x.Type).ToList());
            return Guid.Empty;
        }

        return userId;
    }
    public Guid GetLanguageId()
    {
        if (GetUserId() == default || GetUserId() == Guid.Empty)
        {
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }
        return cacheService.Get($"{CacheConstants.DefaultUserLanguage}_{GetUserId()}", () => dbContext.Set<ApplicationUser>().Where(x => x.Id == GetUserId()).Select(t => t.Language.Id).FirstOrDefault());
    }

    public Guid GetOrganizationId()
    {
        if (GetUserId() == default || GetUserId() == Guid.Empty)
        {
            throw new NotFoundException(LanguageTexts.IdentityUserNotFound);
        }
        return cacheService.Get($"{CacheConstants.DefaultUserOrganization}_{GetUserId()}", () => dbContext.Set<ApplicationUser>().Where(x => x.Id == GetUserId()).Select(t => t.OrganizationId).FirstOrDefault());
    }

}

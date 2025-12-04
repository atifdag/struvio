namespace Struvio.Domain;


public interface IIdentityContext
{

    Guid GetUserId();

    Guid GetLanguageId();

    Guid GetOrganizationId();
}

using Struvio.Common.Models;

namespace Struvio.Application.Services.Abstract;

public interface IAccountService
{
    Task<UserModel> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Guid[]> GetRoleIdsInDefaultOrganizationAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ValidateOrCloseExpiredSessionAsync(Guid userId, CancellationToken cancellationToken = default);
}

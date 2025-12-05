namespace Struvio.Application.Services.Abstract;

public interface IPermissionService : ICrudService<PermissionModel>
{
    Task<RolePermissionLineModel[]> GetAllRolePermissionLinesAsync(CancellationToken cancellationToken = default);
}

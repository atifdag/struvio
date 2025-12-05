namespace Struvio.Common.Models;

public record RolePermissionLineModel(Guid RoleId, string RoleCode, Guid PermissionId, string PermissionCode, string? ControllerName, string? ActionName, string? Path);

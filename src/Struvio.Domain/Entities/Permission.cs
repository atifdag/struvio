
namespace Struvio.Domain.Entities;

public class Permission : IHistoryEntity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string? ControllerName { get; set; }
    public string? ActionName { get; set; }
    public string? Path { get; set; }
    public long SequenceNumber { get; set; }
    public bool IsApproved { get; set; }
    public long Version { get ; set ; }
    public DateTime CreationTime { get ; set ; }
    public DateTime LastModificationTime { get ; set ; }
    public virtual ApplicationUser Creator { get; set; } = null!;
    public virtual ApplicationUser LastModifier { get; set; } = null!;
    public virtual ICollection<RolePermissionLine> RolePermissionLines { get; set; } = [];


}

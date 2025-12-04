namespace Struvio.Domain.Entities;

public class RolePermissionLine : IHistoryEntity
{
    public Guid Id { get; set; }
    public long Version { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
    public virtual ApplicationRole Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
    public virtual ApplicationUser Creator { get; set; } = null!;
    public virtual ApplicationUser LastModifier { get; set; } = null!;

}

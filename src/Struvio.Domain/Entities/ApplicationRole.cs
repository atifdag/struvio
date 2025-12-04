namespace Struvio.Domain.Entities;

public class ApplicationRole : IdentityRole<Guid>, IOrganizationalEntity
{
    public string Code { get; set; } = null!;
    public long SequenceNumber { get; set; }
    public bool IsApproved { get; set; }
    public long Version { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
    public virtual Organization Organization { get; set; } = null!;
    public virtual ApplicationUser Creator { get; set; } = null!;
    public virtual ApplicationUser LastModifier { get; set; } = null!;
    public virtual ICollection<RolePermissionLine> RolePermissionLines { get; set; } = [];
    public virtual ICollection<UserOrganizationRoleLine> UserOrganizationRoleLines { get; set; } = [];
}

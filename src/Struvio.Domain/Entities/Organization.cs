namespace Struvio.Domain.Entities;

public class Organization
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
    public string ApiPassword { get; set; } = null!;
    public string? Description { get; set; }
    public long SequenceNumber { get; set; }
    public bool IsApproved { get; set; }
    public long Version { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }

    public ICollection<ApplicationUser> Users { get; set; } = [];
    public virtual ICollection<UserOrganizationRoleLine> UserOrganizationRoleLines { get; set; } = [];
    public virtual ICollection<ApplicationRole> Roles { get; set; } = [];


}

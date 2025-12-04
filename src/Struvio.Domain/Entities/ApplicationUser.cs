namespace Struvio.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>, IOrganizationalEntity
{
    public Guid PersonId { get; set; }
    public virtual Person Person { get; set; } = null!;
    public Guid OrganizationId { get; set; }
    public virtual Organization Organization { get; set; } = null!;
    public Guid LanguageId { get; set; }
    public virtual Language Language { get; set; } = null!;
    public long SequenceNumber { get; set; }

    public bool IsApproved { get; set; }
    public long Version { get; set; }
   
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
   
    public virtual ApplicationUser Creator { get; set; } = null!;
    public virtual ApplicationUser LastModifier { get; set; } = null!;
    public ICollection<UserOrganizationRoleLine> UserOrganizationRoleLines { get; set; } = [];
    public ICollection<UserSession> CreatedUserSessions { get; set; } = [];

    public ICollection<ApplicationUser> CreatedUsers { get; set; } = [];
    public ICollection<ApplicationUser> LastModifiedUsers { get; set; } = [];

    public ICollection<ApplicationRole> CreatedRoles { get; set; } = [];
    public ICollection<ApplicationRole> LastModifiedRoles { get; set; } = [];

    public ICollection<Permission> CreatedPermissions { get; set; } = [];
    public ICollection<Permission> LastModifiedPermissions { get; set; } = [];

    public ICollection<RolePermissionLine> CreatedRolePermissionLines { get; set; } = [];
    public ICollection<RolePermissionLine> LastModifiedRolePermissionLines { get; set; } = [];

    public ICollection<UserOrganizationRoleLine> CreatedUserOrganizationRoleLines { get; set; } = [];
    public ICollection<UserOrganizationRoleLine> LastModifiedUserOrganizationRoleLines { get; set; } = [];







}

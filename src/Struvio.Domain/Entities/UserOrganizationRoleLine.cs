namespace Struvio.Domain.Entities;

public class UserOrganizationRoleLine : IdentityUserRole<Guid>, IHistoryEntity
{
    public Guid Id { get; set; }
    public long Version { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Organization Organization { get; set; } = null!;
    public virtual ApplicationRole Role { get; set; } = null!;
    public virtual ApplicationUser Creator { get; set; } = null!;
    public virtual ApplicationUser LastModifier { get; set; } = null!;


  
   
}

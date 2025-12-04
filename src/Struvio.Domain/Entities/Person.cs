namespace Struvio.Domain.Entities;

public class Person
{
    public Guid Id { get; set; }
    public string IdentityNumber { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string DisplayName => $"{FirstName} {LastName}";
    public bool IsApproved { get; set; }
    public long Version { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public virtual ApplicationUser? User { get; set; }

    
}

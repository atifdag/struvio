namespace Struvio.Domain.Entities;


public class Language
{
    public Guid Id { get; set; }
    public string ShortCode { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public long SequenceNumber { get; set; }
    public bool IsApproved { get; set; }
    public long Version { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public virtual ICollection<ApplicationUser> Users { get; set; } = [];

}

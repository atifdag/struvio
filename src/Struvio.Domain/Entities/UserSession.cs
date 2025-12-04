namespace Struvio.Domain.Entities;
public class UserSession : IEntity
{
    public Guid Id { get; set; }
    public string? IpAddress { get; set; }
    public string? AgentInfo { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ExpiresAt { get; set; }
    public virtual ApplicationUser Creator { get; set; } = null!;
}

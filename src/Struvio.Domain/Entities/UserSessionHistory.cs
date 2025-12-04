namespace Struvio.Domain.Entities;

public class UserSessionHistory : IEntity
{
    public Guid Id { get; set; }
    public Guid UserSessionId { get; set; }
    public string? IpAddress { get; set; }
    public string? AgentInfo { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public LogoutType LogoutType { get; set; }
}
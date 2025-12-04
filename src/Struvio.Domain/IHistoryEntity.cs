using Struvio.Domain.Entities;


public interface IHistoryEntity : IEntity
{
    long Version { get; set; }
    DateTime CreationTime { get; set; }
    DateTime LastModificationTime { get; set; }
    ApplicationUser Creator { get; set; }
    ApplicationUser LastModifier { get; set; }

}

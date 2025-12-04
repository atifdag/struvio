using Struvio.Domain.Entities;


public interface IOrganizationalEntity : IHistoryEntity
{
    Organization Organization { get; set; }

}

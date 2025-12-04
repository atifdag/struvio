using System.Text.Json;

namespace Struvio.Domain.Entities;

public class History
{
    public Guid Id { get; set; }
    public string EntityName { get; set; } = null!;
    public Guid RowId { get; set; }
    public Guid UserId { get; set; }
    public long Version { get; set; }
    public bool IsDeleted { get; set; }
    public JsonDocument? Data { get; set; }
    public JsonDocument? ChangedData { get; set; }
    public DateTime TransactionTime { get; set; }
}

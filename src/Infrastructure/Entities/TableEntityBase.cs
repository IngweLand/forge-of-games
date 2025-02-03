using Azure;
using Azure.Data.Tables;

namespace Ingweland.Fog.Infrastructure.Entities;

public abstract class TableEntityBase:ITableEntity
{
    public required string PartitionKey { get; set; }
    public required string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}

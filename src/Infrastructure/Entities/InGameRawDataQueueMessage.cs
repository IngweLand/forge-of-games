using Ingweland.Fog.Infrastructure.Enums;

namespace Ingweland.Fog.Infrastructure.Entities;

public class InGameRawDataQueueMessage
{
    public required string PartitionKey { get; init; }
    public required InGameDataProcessingServiceType ProcessingServiceType { get; init; }
    public required string RowKey { get; init; }
}

namespace Ingweland.Fog.Infrastructure.Entities;

public class InGameBinDataTableEntity : InGameBinDataTableEntityBase
{
    public required int PlayerId { get; init; }
    public required string GameWorldId { get; init; }
}

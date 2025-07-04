namespace Ingweland.Fog.Models.Fog.Entities;

public class InGameBinData
{
    public required DateTime CollectedAt { get; init; }
    public required byte[] Data { get; init; }
    public required string DataKey { get; set; }
    public required string GameWorldId { get; init; }
    public required int PlayerId { get; init; }
}

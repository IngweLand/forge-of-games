namespace Ingweland.Fog.Models.Fog.Entities;

public class AllianceRankingRawData
{
    public required byte[] Data { get; init; }
    public required DateTime CollectedAt { get; init; }
}
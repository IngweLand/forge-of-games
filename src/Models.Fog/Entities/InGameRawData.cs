namespace Ingweland.Fog.Models.Fog.Entities;

public class InGameRawData
{
    public required string Base64Data { get; init; }
    public required DateTime CollectedAt { get; init; }
}
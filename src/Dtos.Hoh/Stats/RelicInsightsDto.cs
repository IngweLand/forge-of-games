namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class RelicInsightsDto
{
    public int FromLevel { get; init; }
    public required IReadOnlyCollection<string> Relics { get; init; }
    public required int ToLevel { get; init; }
    public required string UnitId { get; init; }
}

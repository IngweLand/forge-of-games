namespace Ingweland.Fog.Models.Fog.Entities;

public record BattleTimelineEntry
{
    public required string AbilityId { get; init; }
    public required int UnitInBattleId { get; init; }
    public required int TimeMillis { get; init; }
}

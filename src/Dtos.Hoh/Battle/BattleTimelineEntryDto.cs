namespace Ingweland.Fog.Dtos.Hoh.Battle;

public class BattleTimelineEntryDto
{
    public required string AbilityId { get; init; }
    public int TimeSeconds { get; init; }
    public required int UnitInBattleId { get; init; }
}

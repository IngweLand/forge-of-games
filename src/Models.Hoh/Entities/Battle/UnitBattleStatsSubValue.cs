namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

public class UnitBattleStatsSubValue
{
    public required string UnitId { get; init; }
    public float Attack { get; init; }
    public float Defense { get; init; }
    public float Heal { get; init; }
}

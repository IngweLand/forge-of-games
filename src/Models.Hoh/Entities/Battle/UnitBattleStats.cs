namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

public class UnitBattleStats
{
    public required string UnitId { get; init; }
    public float Attack { get; init; }
    public float Defense { get; init; }
    public float Heal { get; init; }

    public required IReadOnlyCollection<UnitBattleStatsSubValue> SubValues { get; init; } =
        new List<UnitBattleStatsSubValue>();
}

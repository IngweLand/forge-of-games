namespace Ingweland.Fog.Dtos.Hoh.Battle;

public class BattleStatsDto
{
    public required int Id { get; set; }

    public required IReadOnlyCollection<BattleSquadStatsDto> PlayerSquads { get; init; } =
        new List<BattleSquadStatsDto>();
    
    public required IReadOnlyCollection<BattleSquadStatsDto> EnemySquads { get; init; } =
        new List<BattleSquadStatsDto>();
}

public class BattleSquadStatsDto
{
    public UnitBattleStatsDto? Hero { get; set; }
    public UnitBattleStatsDto? SupportUnit { get; set; }
}

public class UnitBattleStatsDto
{
    public required string AssetId { get; set; }
    public float Attack { get; set; }
    public float Defense { get; set; }
    public float Heal { get; set; }
    public required string Name { get; init; }
    public required string UnitId { get; set; }
}

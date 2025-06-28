namespace Ingweland.Fog.Dtos.Hoh.Battle;

public class BattleStatsDto
{
    public required IReadOnlyCollection<BattleSquadStatsDto> EnemySquads { get; init; } =
        new List<BattleSquadStatsDto>();

    public required int Id { get; set; }

    public required IReadOnlyCollection<BattleSquadStatsDto> PlayerSquads { get; init; } =
        new List<BattleSquadStatsDto>();
}

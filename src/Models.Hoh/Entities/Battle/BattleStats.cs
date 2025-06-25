namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

public class BattleStats
{
    public required byte[] BattleId { get; set; }
    public IReadOnlyCollection<BattleSquadStats> EnemySquads { get; set; } = new List<BattleSquadStats>();
    public IReadOnlyCollection<BattleSquadStats> PlayerSquads { get; set; } = new List<BattleSquadStats>();
}

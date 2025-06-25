using System.ComponentModel.DataAnnotations.Schema;
using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class BattleStatsEntity
{
    [NotMapped]
    public IEnumerable<BattleSquadStatsEntity> EnemySquads => Squads.Where(s => s.Side == BattleSquadSide.Enemy);

    public int Id { get; set; }

    public required byte[] InGameBattleId { get; set; }

    [NotMapped]
    public IEnumerable<BattleSquadStatsEntity> PlayerSquads => Squads.Where(s => s.Side == BattleSquadSide.Player);

    public ICollection<BattleSquadStatsEntity> Squads { get; set; } = [];
}

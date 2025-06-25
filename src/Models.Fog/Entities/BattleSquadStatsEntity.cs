using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class BattleSquadStatsEntity
{
    public int BattleStatsId { get; set; }

    public UnitBattleStatsEntity? Hero { get; set; }
    public int Id { get; set; }
    public required BattleSquadSide Side { get; set; }
    public UnitBattleStatsEntity? SupportUnit { get; set; }
}

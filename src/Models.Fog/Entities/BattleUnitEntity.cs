using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class BattleUnitEntity
{
    public required string UnitId { get; set; }
    public int Id { get; set; }
    public required int Level { get; set; }
    public required BattleSquadSide Side { get; set; }

    public ICollection<BattleSummaryEntity> Battles { get; set; } = new List<BattleSummaryEntity>();
}

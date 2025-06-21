namespace Ingweland.Fog.Models.Fog.Entities;

public class BattleUnitEntity
{
    public required string UnitId { get; init; }
    public int Id { get; init; }
    public required int Level { get; init; }

    public ICollection<BattleSummaryEntity> Battles { get; set; } = new List<BattleSummaryEntity>();
}

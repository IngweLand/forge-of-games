namespace Ingweland.Fog.Models.Fog.Entities;

public class UnitBattleStatsEntity
{
    public float Attack { get; set; }

    public float Defense { get; set; }
    public float Heal { get; set; }
    public int Id { get; set; }
    public required string UnitId { get; set; }
}

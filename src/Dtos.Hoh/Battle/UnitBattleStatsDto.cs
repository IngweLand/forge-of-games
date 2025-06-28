namespace Ingweland.Fog.Dtos.Hoh.Battle;

public class UnitBattleStatsDto
{
    public required string AssetId { get; set; }
    public float Attack { get; set; }
    public float Defense { get; set; }
    public float Heal { get; set; }
    public required string Name { get; init; }
    public required string UnitId { get; set; }
}

namespace Ingweland.Fog.Models.Fog.Entities;

public class RelicInsightsEntity
{
    public required int FromLevel { get; set; }
    public int Id { get; set; }
    public required ICollection<string> Relics { get; set; }
    public required int ToLevel { get; set; }
    public required string UnitId { get; set; }
}

namespace Ingweland.Fog.Models.Fog.Entities;

public class HeroEquipmentConfiguration
{
    public int? GarmentId { get; set; }
    public int? HandId { get; set; }
    public int? HatId { get; set; }
    public required string HeroId { get; init; }
    public required string Id { get; init; }
    public bool IsInGame { get; init; }
    public int? NeckId { get; set; }
    public int? RingId { get; set; }
}

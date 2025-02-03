namespace Ingweland.Fog.Models.Fog.Entities;

public class HohCityMapEntity
{
    public required string CityEntityId { get; set; }
    public int Id { get; set; }
    public bool IsRotated { get; set; }
    public required int Level { get; set; }
    public string? SelectedProductId { get; set; }
    public string? CustomizationId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

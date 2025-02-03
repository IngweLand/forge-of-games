namespace Ingweland.Fog.Models.Hoh.Entities.City;

public class CityMapEntity
{
    public required string CityEntityId { get; init; }
    public required string? CustomizationId { get; init; }
    public bool IsRotated { get; init; }
    public required int X { get; init; }
    public required int Y { get; init; }
    public required int Level { get; init; }
}

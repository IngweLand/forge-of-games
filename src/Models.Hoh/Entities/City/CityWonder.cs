using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

public class CityWonder
{
    public required WonderId Id { get; init; }
    public bool IsActive { get; init; }
    public int Level { get; init; }
}

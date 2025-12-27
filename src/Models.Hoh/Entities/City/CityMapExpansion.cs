using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

public class CityMapExpansion
{
    public required string Id { get; init; }
    public ExpansionUnlockingType UnlockingType { get; init; }
}

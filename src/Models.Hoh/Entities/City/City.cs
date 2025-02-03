using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

public class City
{
    public required int PlayerCityId { get; init; }
    public required CityId CityId { get; init; }
    public required IReadOnlyCollection<CityMapEntity> MapEntities { get; init; } = new List<CityMapEntity>();
}

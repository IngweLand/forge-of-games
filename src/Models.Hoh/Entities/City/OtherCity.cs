using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

public class OtherCity
{
    public required CityId CityId { get; init; }
    public required IReadOnlyCollection<CityMapEntity> MapEntities { get; init; } = new List<CityMapEntity>();

    public required IReadOnlyCollection<CityMapExpansion> OpenedExpansions { get; init; } =
        new List<CityMapExpansion>();

    public required HohPlayer Player { get; init; }
    public required int PlayerCityId { get; init; }
}

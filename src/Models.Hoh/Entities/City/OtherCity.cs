using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

public class OtherCity :City
{
    public required HohPlayer Player { get; init; }
    public IReadOnlyCollection<CityWonder> Wonders { get; init; } = new List<CityWonder>();
}

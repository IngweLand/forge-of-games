using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class MapAreaFactory : IMapAreaFactory
{
    public MapArea Create(int expansionSize, IReadOnlyCollection<Expansion> expansions)
    {
        return new MapArea(expansionSize, expansions);
    }
}

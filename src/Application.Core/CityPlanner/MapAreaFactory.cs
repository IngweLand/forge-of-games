using AutoMapper;
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Core.CityPlanner;

public class MapAreaFactory(IMapper mapper) : IMapAreaFactory
{
    public MapArea Create(int expansionSize, IReadOnlyCollection<Expansion> expansions,
        HashSet<string> unlockedExpansions, IEnumerable<CityCultureAreaComponent> mapAreaHappinessProviders)
    {
        return new MapArea(expansionSize, expansions, unlockedExpansions,
            mapper.Map<IReadOnlyCollection<MapAreaHappinessProvider>>(mapAreaHappinessProviders));
    }
}

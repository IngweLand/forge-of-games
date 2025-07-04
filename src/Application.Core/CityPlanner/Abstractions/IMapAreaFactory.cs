using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Core.CityPlanner.Abstractions;

public interface IMapAreaFactory
{
    MapArea Create(int expansionSize, IReadOnlyCollection<Expansion> expansions, HashSet<string> unlockedExpansions,
        IEnumerable<CityCultureAreaComponent> mapAreaHappinessProviders);
}

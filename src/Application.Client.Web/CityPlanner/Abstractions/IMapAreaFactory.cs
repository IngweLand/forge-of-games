using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IMapAreaFactory
{
    MapArea Create(int expansionSize, IReadOnlyCollection<Expansion> expansions, HashSet<string> unlockedExpansions,
        IEnumerable<CityCultureAreaComponent> mapAreaHappinessProviders);
}

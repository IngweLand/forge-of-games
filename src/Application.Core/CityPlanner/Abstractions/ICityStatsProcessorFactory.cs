using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Core.CityPlanner.Abstractions;

public interface ICityStatsProcessorFactory
{
    StatsProcessor Create(CityMapStateCore cityMapState,
        IEnumerable<CityCultureAreaComponent> mapAreaHappinessProviders);
}

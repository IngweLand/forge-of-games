using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityStatsProcessorFactory
{
    StatsProcessor Create(CityMapState cityMapState, IEnumerable<CityCultureAreaComponent> mapAreaHappinessProviders);
}

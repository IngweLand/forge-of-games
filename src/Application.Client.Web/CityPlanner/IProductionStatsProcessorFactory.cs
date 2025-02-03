using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public interface IProductionStatsProcessorFactory
{
    ProductionStatsProcessor Create(CityMapState cityMapState);
}

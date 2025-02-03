using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class ProductionStatsProcessorFactory() : IProductionStatsProcessorFactory
{
    public ProductionStatsProcessor Create(CityMapState cityMapState)
    {
        return new ProductionStatsProcessor(cityMapState);
    }
}

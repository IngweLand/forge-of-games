using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class ProductionStatsProcessorFactory() : IProductionStatsProcessorFactory
{
    public ProductionStatsProcessor Create()
    {
        return new ProductionStatsProcessor();
    }
}

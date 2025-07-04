using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats;

public class ProductionStatsProcessorFactory() : IProductionStatsProcessorFactory
{
    public ProductionStatsProcessor Create()
    {
        return new ProductionStatsProcessor();
    }
}

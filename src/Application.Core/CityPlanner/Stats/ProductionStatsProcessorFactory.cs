using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats;

public class ProductionStatsProcessorFactory(ILogger<ProductionStatsProcessor> logger)
    : IProductionStatsProcessorFactory
{
    public ProductionStatsProcessor Create()
    {
        return new ProductionStatsProcessor(logger);
    }
}

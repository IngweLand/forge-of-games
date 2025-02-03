using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityStatsProcessorFactory(IProductionStatsProcessorFactory productionStatsProcessorFactory, ILogger<StatsProcessor> cityStatsProcessorLogger) : ICityStatsProcessorFactory
{
    public StatsProcessor Create(CityMapState cityMapState)
    {
        var productionStatsProcessor = productionStatsProcessorFactory.Create(cityMapState);
        return new StatsProcessor(cityMapState, productionStatsProcessor, cityStatsProcessorLogger);
    }
}

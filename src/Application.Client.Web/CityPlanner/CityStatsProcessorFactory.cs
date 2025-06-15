using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityStatsProcessorFactory(
    IMapper mapper,
    IProductionStatsProcessorFactory productionStatsProcessorFactory,
    ILogger<StatsProcessor> cityStatsProcessorLogger) : ICityStatsProcessorFactory
{
    public StatsProcessor Create(CityMapState cityMapState,
        IEnumerable<CityCultureAreaComponent> mapAreaHappinessProviders)
    {
        var productionStatsProcessor = productionStatsProcessorFactory.Create();
        return new StatsProcessor(cityMapState, productionStatsProcessor, cityStatsProcessorLogger,
            mapper.Map<IReadOnlyCollection<MapAreaHappinessProvider>>(mapAreaHappinessProviders));
    }
}
using AutoMapper;
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Core.CityPlanner;

public class CityStatsProcessorFactory(
    IMapper mapper,
    IProductionStatsProcessorFactory productionStatsProcessorFactory) : ICityStatsProcessorFactory
{
    public StatsProcessor Create(CityMapStateCore cityMapState,
        IEnumerable<CityCultureAreaComponent> mapAreaHappinessProviders)
    {
        var productionStatsProcessor = productionStatsProcessorFactory.Create();
        return new StatsProcessor(cityMapState, productionStatsProcessor,
            mapper.Map<IReadOnlyCollection<MapAreaHappinessProvider>>(mapAreaHappinessProviders));
    }
}

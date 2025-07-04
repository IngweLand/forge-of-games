using Ingweland.Fog.Application.Core.CityPlanner.Stats;

namespace Ingweland.Fog.Application.Core.CityPlanner.Abstractions;

public interface IProductionStatsProcessorFactory
{
    ProductionStatsProcessor Create();
}

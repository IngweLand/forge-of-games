using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IProductionStatsViewModelFactory
{
    ProductionStatsViewModel Create(IDictionary<string, ConsolidatedCityProduct> products);
}

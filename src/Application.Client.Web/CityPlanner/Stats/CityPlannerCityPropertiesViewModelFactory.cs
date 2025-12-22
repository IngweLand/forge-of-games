using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityPlannerCityPropertiesViewModelFactory(
    IHappinessStatsViewModelFactory happinessStatsViewModelFactory,
    IProductionStatsViewModelFactory productionStatsViewModelFactory,
    IAreaStatsViewModelFactory areaStatsViewModelFactory,
    IWorkerIconUrlProvider workerIconUrlProvider)
    : ICityPlannerCityPropertiesViewModelFactory
{
    public CityPlannerCityPropertiesViewModel Create(CityId cityId, string name, AgeViewModel age, CityStats stats,
        IEnumerable<BuildingDto> buildings, int wonderLevel = 0)
    {
        return new CityPlannerCityPropertiesViewModel
        {
            CityId = cityId,
            Name = name,
            Age = age,
            Workforce = new IconLabelItemViewModel
            {
                Label = $"{stats.ProvidedWorkersCount - stats.RequiredWorkersCount}/{stats.ProvidedWorkersCount}",
                IconUrl = workerIconUrlProvider.GetIcon(cityId),
            },
            Happiness = happinessStatsViewModelFactory.Create(stats),
            Production = productionStatsViewModelFactory.Create(stats.Products, stats.ProductionCosts),
            Areas = areaStatsViewModelFactory.Create(stats, buildings),
            WonderLevel = wonderLevel,
        };
    }
}

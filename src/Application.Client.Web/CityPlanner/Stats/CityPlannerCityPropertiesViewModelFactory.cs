using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityPlannerCityPropertiesViewModelFactory(
    IAssetUrlProvider assetUrlProvider,
    IHappinessStatsViewModelFactory happinessStatsViewModelFactory,
    IProductionStatsViewModelFactory productionStatsViewModelFactory,
    IAreaStatsViewModelFactory areaStatsViewModelFactory)
    : ICityPlannerCityPropertiesViewModelFactory
{
    public CityPlannerCityPropertiesViewModel Create(CityId cityId, string name, AgeViewModel age, CityStats stats,
        IEnumerable<BuildingDto> buildings)
    {
        return new CityPlannerCityPropertiesViewModel()
        {
            Name = name,
            Age = age,
            Workforce = new IconLabelItemViewModel()
            {
                Label = $"{stats.ProvidedWorkersCount - stats.RequiredWorkersCount}/{stats.ProvidedWorkersCount}",
                IconUrl = assetUrlProvider.GetHohWorkerIconUrl(cityId),
            },
            Happiness = happinessStatsViewModelFactory.Create(stats),
            Production = productionStatsViewModelFactory.Create(stats.Products),
            Areas = areaStatsViewModelFactory.Create(stats, buildings)
        };
    }
}

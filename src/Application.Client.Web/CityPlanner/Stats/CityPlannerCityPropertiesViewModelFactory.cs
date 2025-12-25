using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityPlannerCityPropertiesViewModelFactory(
    IHappinessStatsViewModelFactory happinessStatsViewModelFactory,
    IProductionStatsViewModelFactory productionStatsViewModelFactory,
    IAreaStatsViewModelFactory areaStatsViewModelFactory,
    IWorkerIconUrlProvider workerIconUrlProvider,
    IHohResourceIconUrlProvider resourceIconUrlProvider,
    IToolsUiService toolsUiService)
    : ICityPlannerCityPropertiesViewModelFactory
{
    public CityPlannerCityPropertiesViewModel Create(CityId cityId, string name, AgeViewModel age, CityStats stats,
        IEnumerable<BuildingDto> buildings, WonderDto? wonder = null, int wonderLevel = 0)
    {
        var wonderBonus = new List<IconLabelItemViewModel>();
        if (stats.WonderWorkersBonus > 0)
        {
            wonderBonus.Add(new IconLabelItemViewModel
            {
                Label = stats.WonderWorkersBonus.ToString(),
                IconUrl = workerIconUrlProvider.GetIcon(cityId),
            });
        }

        if (stats.WonderResourcesBonus != null)
        {
            wonderBonus.AddRange(stats.WonderResourcesBonus.Select(kvp =>
                new IconLabelItemViewModel
                {
                    Label = $"{kvp.Value:N2}%",
                    IconUrl = resourceIconUrlProvider.GetIconUrl(kvp.Key),
                }));
        }

        string? wonderNextLevelRangeLabel = null;
        IReadOnlyCollection<IconLabelItemViewModel>? wonderCost = null;
        if (wonder != null)
        {
            if (wonderLevel < HohConstants.WONDER_MAX_LEVEL)
            {
                wonderNextLevelRangeLabel = $"{wonderLevel} > {wonderLevel + 1}";
                wonderCost = toolsUiService.CalculateWonderLevelsCost(wonder, wonderLevel, wonderLevel + 1);
            }
            else
            {
                wonderNextLevelRangeLabel = wonderLevel.ToString();
                wonderCost = [];
            }
        }

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
            WonderBonus = wonderBonus.Count > 0 ? wonderBonus : null,
            WonderNextLevelRangeLabel = wonderNextLevelRangeLabel,
            WonderCost = wonderCost,
        };
    }
}

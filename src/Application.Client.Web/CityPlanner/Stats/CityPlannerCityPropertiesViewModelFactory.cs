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
        if (stats.WonderWorkersBonus != null)
        {
            wonderBonus.AddRange(stats.WonderWorkersBonus
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => new IconLabelItemViewModel
                {
                    Label = kvp.Value.ToString(),
                    IconUrl = workerIconUrlProvider.GetIcon(cityId, kvp.Key),
                }));
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

        var workforce = stats.ProvidedWorkers.ToDictionary(worker => worker.Key,
            worker => (Provided: worker.Value, Required: stats.RequiredWorkers.GetValueOrDefault(worker.Key, 0)));

        foreach (var kvp in stats.RequiredWorkers)
        {
            if (!workforce.ContainsKey(kvp.Key))
            {
                workforce.Add(kvp.Key, (0, kvp.Value));
            }
        }

        return new CityPlannerCityPropertiesViewModel
        {
            CityId = cityId,
            Name = name,
            Age = age,
            Workforce = workforce
                .OrderBy(x => x.Key)
                .Select(x => new IconLabelItemViewModel
                {
                    Label = $"{x.Value.Provided - x.Value.Required}/{x.Value.Provided}",
                    IconUrl = workerIconUrlProvider.GetIcon(cityId, x.Key),
                })
                .ToList(),
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

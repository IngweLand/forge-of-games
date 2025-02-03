using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Tools;
using Ingweland.Fog.Application.Core.Formatters;
using Ingweland.Fog.Application.Core.Formatters.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class BuildingMultilevelCostViewModelFactory(
    IHohResourceIconUrlProvider resourceIconUrlProvider,
    ITimeFormatters timeFormatters) : IBuildingMultilevelCostViewModelFactory
{
    public BuildingMultilevelCostViewModel Create(BuildingMultilevelCost cost)
    {
        var uniqueResources = cost.UpgradeCost.Cost.Concat(cost.ConstructionCost.Cost).GroupBy(ra => ra.ResourceId)
            .Select(g => g.Key);
        var constructionCosts = cost.ConstructionCost.Cost.ToDictionary(ra => ra.ResourceId);
        var upgradeCosts = cost.UpgradeCost.Cost.ToDictionary(ra => ra.ResourceId);
        return new BuildingMultilevelCostViewModel()
        {
            FromLevel = cost.CurrentLevel,
            ToLevel = cost.ToLevel,
            Costs = new List<ConstructVsUpgradeItemViewModel>()
            {
                new()
                {
                    IconUrl = resourceIconUrlProvider.GetIconUrl("time"),
                    ConstructionCost = timeFormatters.FromSeconds(cost.ConstructionCost.Time),
                    UpgradeCost = timeFormatters.FromSeconds(cost.UpgradeCost.Time),
                },
            }.Concat(uniqueResources.Select(r =>
                CreateItem(r, constructionCosts: constructionCosts, upgradeCosts: upgradeCosts))).ToList(),
        };
    }

    private ConstructVsUpgradeItemViewModel CreateItem(string resourceId,
        IDictionary<string, ResourceAmount> constructionCosts, IDictionary<string, ResourceAmount> upgradeCosts)
    {
        constructionCosts.TryGetValue(resourceId, out var constructionAmount);
        upgradeCosts.TryGetValue(resourceId, out var upgradeAmount);
        return new ConstructVsUpgradeItemViewModel()
        {
            IconUrl = resourceIconUrlProvider.GetIconUrl(resourceId),
            ConstructionCost = constructionAmount != null ? constructionAmount.Amount.ToString("N0") : "-",
            UpgradeCost = upgradeAmount != null ? upgradeAmount.Amount.ToString("N0") : "-",
        };
    }
}

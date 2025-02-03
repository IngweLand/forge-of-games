using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Core.Calculators;

public class CityCalculators : ICityCalculators
{
    public BuildingMultilevelCost CalculateMultilevelCost(IEnumerable<BuildingLevelCostData> levelCosts,
        int currentLevel, int toLevel)
    {
        if (currentLevel > toLevel)
        {
            throw new ArgumentException("Target level must be greater than or equal to from level.", nameof(toLevel));
        }

        var levelCostList = levelCosts.ToList();
        var constructionLevel = FindConstructionLevel(levelCostList, toLevel);
        BuildingLevelCost constructionCost;
        BuildingLevelCost upgradeCost;
        if (constructionLevel > 0)
        {
            var constructionLevels = levelCostList
                .Where(lcd => lcd.Level >= constructionLevel && lcd.Level <= toLevel)
                .ToList();
            constructionCost = CalculateConstructionCost(constructionLevels);

            upgradeCost = currentLevel > 0
                ? CalculateUpgradeCost(levelCostList, currentLevel + 1, toLevel)
                : BuildingLevelCost.Blank;
        }
        else
        {
            constructionCost = BuildingLevelCost.Blank;
            upgradeCost = CalculateUpgradeCost(levelCostList, currentLevel + 1, toLevel);
        }

        return new BuildingMultilevelCost
        {
            CurrentLevel = currentLevel,
            ToLevel = toLevel,
            ConstructionCost = constructionCost,
            UpgradeCost = upgradeCost,
        };
    }

    public IReadOnlyCollection<ResourceAmount> CalculateWonderLevelsCost(WonderDto wonderDto, int currentLevel,
        int targetLevel)
    {
        var cumulativeCratesCount = 0;
        var cumulativeCost = new List<WonderCrate>();
        for (var index = currentLevel; index < targetLevel; index++)
        {
            var dtoLevel = wonderDto.Levels[index];
            var consolidatedCost = ConsolidateCrates(dtoLevel.Cost);
            cumulativeCost = AddCrates(cumulativeCost.Concat(consolidatedCost));
            var cratesCount = consolidatedCost.Sum(wc => wc.Amount);
            cumulativeCratesCount += cratesCount;
        }

        return new List<ResourceAmount>()
        {
            new()
            {
                ResourceId = "crate",
                Amount = cumulativeCratesCount,
            },
        }.Concat(cumulativeCost.Select(wc => wc.FillResource)).ToList();
    }

    private List<WonderCrate> AddCrates(IEnumerable<WonderCrate> src)
    {
        return src
            .GroupBy(obj => obj.FillResource.ResourceId)
            .Select(group => new WonderCrate
            {
                Amount = group.Sum(obj => obj.Amount),
                FillResource = new ResourceAmount
                {
                    ResourceId = group.Key,
                    Amount = group.Sum(obj => obj.FillResource.Amount),
                },
            })
            .ToList();
    }

    private List<WonderCrate> ConsolidateCrates(IEnumerable<WonderCrate> src)
    {
        return src
            .GroupBy(obj => obj.FillResource.ResourceId)
            .Select(group => new WonderCrate
            {
                Amount = group.Sum(obj => obj.Amount),
                FillResource = new ResourceAmount
                {
                    ResourceId = group.Key,
                    Amount = group.Sum(obj => obj.Amount * obj.FillResource.Amount),
                },
            })
            .ToList();
    }

    private BuildingLevelCost CalculateConstructionCost(List<BuildingLevelCostData> constructionLevels)
    {
        var initialCost = new BuildingLevelCost
        {
            Cost = constructionLevels[0].ConstructionComponent!.Cost,
            Time = constructionLevels[0].ConstructionComponent!.BuildTime,
        };

        if (constructionLevels.Count <= 1)
        {
            return initialCost;
        }

        var upgradeConstructionLevels = constructionLevels[1..].Select(lcd => lcd.UpgradeComponent!).ToList();
        var upgradeConstructionCost = new BuildingLevelCost
        {
            Time = upgradeConstructionLevels.Sum(uc => uc.UpgradeTime),
            Cost = AggregateResourceCosts(upgradeConstructionLevels.SelectMany(uc => uc.Cost)),
        };

        return new BuildingLevelCost
        {
            Time = initialCost.Time + upgradeConstructionCost.Time,
            Cost = AggregateResourceCosts(initialCost.Cost.Concat(upgradeConstructionCost.Cost)),
        };
    }

    private BuildingLevelCost CalculateUpgradeCost(List<BuildingLevelCostData> levelCosts, int fromLevel,
        int toLevel)
    {
        var upgradeLevels = levelCosts
            .Where(lcd => lcd.UpgradeComponent != null && lcd.Level >= fromLevel && lcd.Level <= toLevel)
            .Select(lcd => lcd.UpgradeComponent!)
            .ToList();

        return new BuildingLevelCost
        {
            Time = upgradeLevels.Sum(uc => uc.UpgradeTime),
            Cost = AggregateResourceCosts(upgradeLevels.SelectMany(uc => uc.Cost)),
        };
    }

    private List<ResourceAmount> AggregateResourceCosts(IEnumerable<ResourceAmount> costs)
    {
        return costs
            .GroupBy(r => r.ResourceId)
            .Select(g => new ResourceAmount
            {
                ResourceId = g.Key,
                Amount = g.Sum(r => r.Amount),
            })
            .ToList();
    }

    private static int FindConstructionLevel(List<BuildingLevelCostData> levelsData, int targetLevel)
    {
        var constructionLevelData = levelsData
            .Where(ld => ld.Level <= targetLevel && ld.ConstructionComponent != null)
            .OrderByDescending(ld => ld.Level)
            .FirstOrDefault();

        return constructionLevelData?.Level ?? 0;
    }
}

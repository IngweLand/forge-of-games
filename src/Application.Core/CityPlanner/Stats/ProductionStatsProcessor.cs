using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats;

public class ProductionStatsProcessor : IProductionStatsProcessor
{
    private const int SECONDS_IN_HOUR = 3600;
    private const int SECONDS_IN_DAY = 24 * 3600;

    public void UpdateProduction(CityMapEntity cityMapEntity)
    {
        UpdateProduction(cityMapEntity, new Dictionary<string, double>());
    }

    public void UpdateProduction(CityMapEntity cityMapEntity, IReadOnlyDictionary<string, double> modifiers)
    {
        var productionProvider = cityMapEntity.FirstOrDefaultStat<ProductionProvider>();
        if (productionProvider == null)
        {
            return;
        }

        if (cityMapEntity.ExcludeFromStats)
        {
            productionProvider.ProductionStatsItems = [];
            return;
        }

        var happinessConsumer = cityMapEntity.FirstOrDefaultStat<HappinessConsumer>();
        var productionItems = new List<ProductionStatsItem>();
        foreach (var productionComponent in productionProvider.ProductionComponents)
        {
            var products = new List<ProductStatsItem>();
            var defaultProductionHours = (float) productionComponent.ProductionTime / SECONDS_IN_HOUR;
            foreach (var resourceProduct in productionComponent.Products)
            {
                if (resourceProduct is ResourceReward resourceReward)
                {
                    products.Add(ProcessResourceReward(resourceReward, modifiers, happinessConsumer,
                        defaultProductionHours, productionComponent.ProductionTime));
                }
                else if (resourceProduct is Reward complexReward)
                {
                }
            }

            var cost = productionComponent.Cost.Select(resourceAmount => new ProductionCostStatsItem
                {
                    ResourceId = resourceAmount.ResourceId,
                    Default = resourceAmount.Amount,
                    OneHour = (int) (resourceAmount.Amount / defaultProductionHours),
                    OneDay = (int) (resourceAmount.Amount / defaultProductionHours * 24),
                })
                .ToList();

            var extraCost = new Dictionary<string, ProductionCostStatsItem>();
            foreach (var costItem in cost)
            {
                if (!ResourceConversions.Rates.TryGetValue(costItem.ResourceId, out var conversionRates))
                {
                    continue;
                }

                foreach (var t in conversionRates)
                {
                    if (extraCost.TryGetValue(t.SourceResourceId, out var extraCostItem))
                    {
                        extraCost[t.SourceResourceId] = new ProductionCostStatsItem
                        {
                            ResourceId = t.SourceResourceId,
                            Default = (int) Math.Ceiling(costItem.Default / t.TargetPerSource) + extraCostItem.Default,
                            OneHour = (int) Math.Ceiling(costItem.OneHour / t.TargetPerSource) + extraCostItem.OneHour,
                            OneDay = (int) Math.Ceiling(costItem.OneDay / t.TargetPerSource) + extraCostItem.OneDay,
                        };
                    }
                    else
                    {
                        extraCost.Add(t.SourceResourceId, new ProductionCostStatsItem
                        {
                            ResourceId = t.SourceResourceId,
                            Default = (int) Math.Ceiling(costItem.Default / t.TargetPerSource),
                            OneHour = (int) Math.Ceiling(costItem.OneHour / t.TargetPerSource),
                            OneDay = (int) Math.Ceiling(costItem.OneDay / t.TargetPerSource),
                        });
                    }
                }
            }

            cost.AddRange(extraCost.Values);

            productionItems.Add(new ProductionStatsItem
            {
                ProductionId = productionComponent.Id,
                Products = products,
                WorkerBehaviour = productionComponent.WorkerBehaviour,
                Cost = cost,
            });
        }

        productionProvider.ProductionStatsItems = productionItems;
    }

    private static ProductStatsItem ProcessResourceReward(ResourceReward reward,
        IReadOnlyDictionary<string, double> modifiers, HappinessConsumer? happinessConsumer,
        float defaultProductionHours, int defaultProductionTime)
    {
        var defaultProductionAmount = reward.Amount;
        var totalProductionAmount = (double) defaultProductionAmount;
        if (modifiers.TryGetValue(reward.ResourceId, out var modifier))
        {
            totalProductionAmount += totalProductionAmount * (modifier / 100);
        }

        if (happinessConsumer != null)
        {
            var buffedResource =
                happinessConsumer.BuffDetails.Resources?.FirstOrDefault(r =>
                    r.ResourceId == reward.ResourceId);
            double factor;
            if (buffedResource != null)
            {
                factor = MathF.Min(1,
                        (float) happinessConsumer.ConsumedHappiness / happinessConsumer.BuffDetails.Value) *
                    buffedResource.Factor;
            }
            else
            {
                factor = MathF.Min(1,
                        (float) happinessConsumer.ConsumedHappiness / happinessConsumer.BuffDetails.Value) *
                    happinessConsumer.BuffDetails.Factor;
            }

            var oneHourBonus = (int) Math.Floor(happinessConsumer.BuffDetails.Value * factor);
            totalProductionAmount += oneHourBonus * defaultProductionHours;
        }

        return new ProductStatsItem
        {
            ResourceId = reward.ResourceId,
            DefaultProduction = new TimedProductStatsItem
            {
                ProductionTime = defaultProductionTime,
                Value = defaultProductionAmount,
                BuffedValue = (int) totalProductionAmount,
            },

            OneHourProduction = new TimedProductStatsItem
            {
                ProductionTime = SECONDS_IN_HOUR,
                Value = (int) (defaultProductionAmount / defaultProductionHours),
                BuffedValue = (int) (totalProductionAmount / defaultProductionHours),
            },

            OneDayProduction = new TimedProductStatsItem
            {
                ProductionTime = SECONDS_IN_DAY,
                Value = (int) (defaultProductionAmount / defaultProductionHours * 24),
                BuffedValue = (int) (totalProductionAmount / defaultProductionHours * 24),
            },
        };
    }
}

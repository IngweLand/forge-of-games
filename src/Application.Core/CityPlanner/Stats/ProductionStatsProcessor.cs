using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats;

public class ProductionStatsProcessor(ILogger<ProductionStatsProcessor> logger) : IProductionStatsProcessor
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
            var products = new List<List<ProductStatsItem>>();
            var defaultProductionHours = (float) productionComponent.ProductionTime / SECONDS_IN_HOUR;
            foreach (var productReward in productionComponent.Products)
            {
                var rewards = new List<ProductStatsItem>();
                if (productReward is ResourceReward resourceReward)
                {
                    rewards.Add(ProcessResourceReward(resourceReward, modifiers, happinessConsumer,
                        defaultProductionHours, productionComponent.ProductionTime));
                }
                else if (productReward is Reward complexReward)
                {
                    try
                    {
                        rewards.AddRange(ProcessComplexReward(complexReward, modifiers, happinessConsumer,
                            defaultProductionHours, productionComponent.ProductionTime));
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Failed to process complex reward.");
                    }
                }

                products.Add(rewards);
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

    private static double CalculateRewardExpectedValue(MysteryChestReward chest)
    {
        var rewards = chest.Rewards.Cast<ResourceReward>().ToArray();
        var probabilities = chest.Probabilities.ToArray();
        if (rewards.Length != probabilities.Length)
        {
            throw new InvalidOperationException("Rewards and probabilities must match.");
        }

        var totalWeight = chest.Probabilities.Sum();
        double total = 0;
        for (var i = 0; i < rewards.Length; i++)
        {
            total += rewards[i].Amount * probabilities[i];
        }

        return total / totalWeight;
    }

    private static List<ProductStatsItem> ProcessComplexReward(Reward reward,
        IReadOnlyDictionary<string, double> modifiers, HappinessConsumer? happinessConsumer,
        float defaultProductionHours, int defaultProductionTime)
    {
        // We currently support only Vikings production, where the structure is: 
        // RewardDefinitionDTO > LootContainerRewardDTO > RewardDefinitionDTO > MysteryChestRewardDTO
        // We also support only cases where the Mysterious chest has only one type of resource.
        var rewards = new List<ProductStatsItem>();
        var lootContainerReward = reward.Rewards.OfType<LootContainerReward>().Single();
        foreach (var rewardWrapper in lootContainerReward.Rewards.OfType<Reward>())
        {
            var mysteryChestReward = rewardWrapper.Rewards.OfType<MysteryChestReward>().Single();
            var concreteRewards = mysteryChestReward.Rewards.OfType<ResourceReward>().ToList();
            var resourceIds = concreteRewards.Select(x => x.ResourceId).ToHashSet();
            if (resourceIds.Count > 1)
            {
                throw new InvalidOperationException("MysteryChestReward with multiple resources is not supported.");
            }

            var resourceId = resourceIds.First();
            var defaultProductionAmount = CalculateRewardExpectedValue(mysteryChestReward);
            var totalProductionAmount = defaultProductionAmount;
            if (modifiers.TryGetValue(resourceId, out var modifier))
            {
                totalProductionAmount += totalProductionAmount * (modifier / 100);
            }

            if (happinessConsumer != null)
            {
                var buffedResource =
                    happinessConsumer.BuffDetails.Resources?.FirstOrDefault(r =>
                        r.ResourceId == resourceId);
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

            rewards.Add(new ProductStatsItem
            {
                ResourceId = resourceId,
                DefaultProduction = new TimedProductStatsItem
                {
                    ProductionTime = defaultProductionTime,
                    Value = (int) Math.Round(defaultProductionAmount, MidpointRounding.AwayFromZero),
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
            });
        }

        return rewards;
    }
}

using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.Application.Core.CityPlanner;

public class ProductInfoFactory : IProductInfoFactory
{
    public ProductInfo Create(ProductionComponent productionComponent)
    {
        return new ProductInfo(productionComponent.Id,
            productionComponent.ProductionTime, GetResourceIds(productionComponent.Products));
    }

    private static Dictionary<string, string> GetResourceIds(IReadOnlyCollection<RewardBase> products)
    {
        var resourceIds = new Dictionary<string, string>();
        foreach (var productReward in products)
        {
            if (productReward is ResourceReward resourceReward)
            {
                resourceIds.TryAdd(resourceReward.ResourceId, HohStringParser.GetConcreteId(resourceReward.ResourceId));
            }
            else if (productReward is Reward complexReward)
            {
                try
                {
                    var lootContainerReward = complexReward.Rewards.OfType<LootContainerReward>().Single();
                    foreach (var rewardWrapper in lootContainerReward.Rewards.OfType<Reward>())
                    {
                        var mysteryChestReward = rewardWrapper.Rewards.OfType<MysteryChestReward>().Single();
                        var concreteRewards = mysteryChestReward.Rewards.OfType<ResourceReward>().ToList();
                        foreach (var concreteReward in concreteRewards)
                        {
                            resourceIds.TryAdd(concreteReward.ResourceId,
                                HohStringParser.GetConcreteId(concreteReward.ResourceId));
                        }
                    }
                }
                catch (Exception e)
                {
                    // ignore
                }
            }
        }

        return resourceIds;
    }
}

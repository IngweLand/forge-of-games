using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class ProductionStatsProcessor(CityMapState cityMapState) : IProductionStatsProcessor
{
    private const int SECONDS_IN_HOUR = 3600;
    private const int SECONDS_IN_DAY = 24 * 3600;

    public void UpdateProduction(CityMapEntity cityMapEntity, CityMapState cityMapState)
    {
        var productionProvider = cityMapEntity.FirstOrDefaultStat<ProductionProvider>();
        if (productionProvider == null)
        {
            return;
        }

        var happinessConsumer = cityMapEntity.FirstOrDefaultStat<HappinessConsumer>();
        var productionItems = new List<ProductionStatsItem>();
        foreach (var productionComponent in productionProvider.ProductionComponents)
        {
            var products = new List<ProductStatsItem>();
            foreach (var resourceProduct in productionComponent.Products.OfType<ResourceReward>())
            {
                var defaultProductionAmount = resourceProduct.Amount;
                var buffedProductionAmount = 0;
                var hours = productionComponent.ProductionTime / SECONDS_IN_HOUR;
                if (happinessConsumer != null)
                {
                    var buffedResource =
                        happinessConsumer.BuffDetails.Resources?.FirstOrDefault(r =>
                            r.ResourceId == resourceProduct.ResourceId);
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
                    buffedProductionAmount = defaultProductionAmount + oneHourBonus * hours;
                }
                else
                {
                    buffedProductionAmount = defaultProductionAmount;
                }

                products.Add(new ProductStatsItem()
                {
                    ResourceId = resourceProduct.ResourceId,
                    DefaultProduction = new TimedProductStatsItem()
                    {
                        ProductionTime = productionComponent.ProductionTime,
                        Value = defaultProductionAmount,
                        BuffedValue = buffedProductionAmount,
                    },

                    OneHourProduction = new TimedProductStatsItem()
                    {
                        ProductionTime = SECONDS_IN_HOUR,
                        Value = defaultProductionAmount / hours,
                        BuffedValue = buffedProductionAmount / hours,
                    },

                    OneDayProduction = new TimedProductStatsItem()
                    {
                        ProductionTime = SECONDS_IN_DAY,
                        Value = defaultProductionAmount / hours * 24,
                        BuffedValue = buffedProductionAmount / hours * 24,
                    },
                });
            }

            productionItems.Add(new ProductionStatsItem()
            {
                ProductionId = productionComponent.Id,
                Products = products,
                WorkerCount = productionComponent.WorkerCount,
            });
        }

        productionProvider.ProductionStatsItems = productionItems;
    }
}

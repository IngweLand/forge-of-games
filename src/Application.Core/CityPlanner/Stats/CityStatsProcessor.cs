using Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats;

public static class CityStatsProcessor
{
    public static CityStats Update(IEnumerable<CityMapEntity> entities,
        IEnumerable<MapAreaHappinessProvider> mapAreaHappinessProviders,
        IEnumerable<CityMapExpansion> openExpansions,
        int additionalWorkers = 0)
    {
        var stats = new CityStats
        {
            ProvidedWorkersCount = additionalWorkers,
        };

        foreach (var cme in entities)
        {
            if (cme.ExcludeFromStats)
            {
                continue;
            }

            var workerProvider = cme.FirstOrDefaultStat<WorkerProvider>();
            if (workerProvider != null)
            {
                stats.ProvidedWorkersCount += workerProvider.WorkerCount;
            }

            var productionProvider = cme.FirstOrDefaultStat<ProductionProvider>();
            if (productionProvider != null)
            {
                var canSelectProduct = ProductionProviderHelper.CanSelectProduct(cme.BuildingType, cme.BuildingGroup);
                if (canSelectProduct)
                {
                    if (cme.SelectedProductId != null)
                    {
                        stats.RequiredWorkersCount += productionProvider.ProductionStatsItems
                            .FirstOrDefault(psi => psi.ProductionId == cme.SelectedProductId)?.WorkerCount ?? 0;
                    }
                }
                else
                {
                    stats.RequiredWorkersCount +=
                        productionProvider.ProductionStatsItems.FirstOrDefault()?.WorkerCount ?? 0;
                }

                foreach (var productionStatsItem in productionProvider.ProductionStatsItems)
                {
                    if (!canSelectProduct ||
                        (canSelectProduct && productionStatsItem.ProductionId == cme.SelectedProductId))
                    {
                        foreach (var productStatsItem in productionStatsItem.Products)
                        {
                            if (!stats.Products.TryGetValue(productStatsItem.ResourceId, out var productTuple))
                            {
                                productTuple = new ConsolidatedTimedProductionValues();
                                stats.Products.Add(productStatsItem.ResourceId, productTuple);
                            }

                            productTuple.Default += productStatsItem.DefaultProduction.BuffedValue;
                            productTuple.OneHour += productStatsItem.OneHourProduction.BuffedValue;
                            productTuple.OneDay += productStatsItem.OneDayProduction.BuffedValue;
                        }

                        foreach (var costItem in productionStatsItem.Cost)
                        {
                            if (!stats.ProductionCosts.TryGetValue(costItem.ResourceId, out var costTuple))
                            {
                                costTuple = new ConsolidatedTimedProductionValues();
                                stats.ProductionCosts.Add(costItem.ResourceId, costTuple);
                            }

                            costTuple.Default += costItem.Default;
                            costTuple.OneHour += costItem.OneHour;
                            costTuple.OneDay += costItem.OneDay;
                        }
                    }
                }
            }

            var happinessProvider = cme.FirstOrDefaultStat<HappinessProvider>();
            if (happinessProvider != null)
            {
                stats.TotalAvailableHappiness += happinessProvider.Value;
            }

            var happinessConsumer = cme.FirstOrDefaultStat<HappinessConsumer>();
            if (happinessConsumer != null)
            {
                stats.TotalHappinessConsumption += happinessConsumer.ConsumedHappiness;
                if (happinessConsumer.ConsumedHappiness > happinessConsumer.BuffDetails.Value)
                {
                    stats.ExcessHappiness +=
                        happinessConsumer.ConsumedHappiness - happinessConsumer.BuffDetails.Value;
                }
                else
                {
                    stats.UnmetHappinessNeed +=
                        happinessConsumer.BuffDetails.Value - happinessConsumer.ConsumedHappiness;
                }
            }

            var areaProvider = cme.FirstOrDefaultStat<AreaProvider>();
            if (areaProvider != null)
            {
                if (!stats.AreasByType.TryGetValue(areaProvider.BuildingType, out var typedArea))
                {
                    stats.AreasByType.Add(areaProvider.BuildingType, 0);
                }

                typedArea += areaProvider.Area;
                stats.AreasByType[areaProvider.BuildingType] = typedArea;

                if (!stats.AreasByGroup.TryGetValue(areaProvider.BuildingGroup, out var groupedArea))
                {
                    stats.AreasByGroup.Add(areaProvider.BuildingGroup, 0);
                }

                groupedArea += areaProvider.Area;
                stats.AreasByGroup[areaProvider.BuildingGroup] = groupedArea;
            }
        }

        var areaHappinessList = mapAreaHappinessProviders.ToList();
        var expansions = openExpansions.Select(x => x.Bounds).ToList();
        var intersecting = areaHappinessList
            .Where(x => expansions.Any(y => y.IntersectsWith(x.Bounds)))
            .ToList();
        stats.TotalAvailableHappiness += intersecting.Sum(src => src.Value);
        stats.TotalArea = expansions.Sum(x => x.Width * x.Height);

        return stats;
    }
}

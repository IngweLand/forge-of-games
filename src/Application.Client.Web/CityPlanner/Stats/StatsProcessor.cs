using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class StatsProcessor(
    CityMapState cityMapState,
    IProductionStatsProcessor productionStatsProcessor,
    ILogger<StatsProcessor> logger)
{
    private void UpdateHappiness()
    {
        foreach (var target in cityMapState.HappinessConsumers)
        {
            UpdateHappiness(target);
        }
    }

    private void UpdateHappiness(CityMapEntity target)
    {
        var happinessConsumer = target.FirstOrDefaultStat<HappinessConsumer>();
        if (happinessConsumer == null)
        {
            return;
        }

        var intersections =
            cityMapState.HappinessProviders.Where(hp =>
                !hp.ExcludeFromStats && target.Bounds.IntersectsWith(hp.OverflowBounds!.Value)).ToList();
        var age = cityMapState.CityAge;
        var happiness = intersections.Sum(cme =>
            cityMapState.Buildings[cme.CityEntityId].CultureComponent!.GetValue(age.Id, cme.Level));
        happinessConsumer.ConsumedHappiness = happiness;
        target.HappinessFraction =
            (float) happiness / cityMapState.Buildings[target.CityEntityId].BuffDetails!.Value;
    }

    public void UpdateStats()
    {
        UpdateEvolvingBuildings();
        UpdateHappiness();
        UpdateProduction();
        cityMapState.CityStats = CityStatsProcessor.Update(cityMapState.CityMapEntities);
    }

    private void UpdateEvolvingBuildings()
    {
        var evolvingBuildings = cityMapState.TypedEntities.Where(kvp => kvp.Key == BuildingType.Evolving)
            .SelectMany(kvp => kvp.Value);
        foreach (var cme in evolvingBuildings)
        {
            cme.FirstOrDefaultStat<HappinessProvider>()?.Update(cityMapState.CityAge.Id, cme.Level);
        }
    }

    private void UpdateProduction()
    {
        foreach (var cme in cityMapState.CityMapEntities)
        {
            var modifiers = GetModifiers();
            if (modifiers == null)
            {
                productionStatsProcessor.UpdateProduction(cme);
            }
            else
            {
                productionStatsProcessor.UpdateProduction(cme, modifiers);
            }
        }
    }

    private IReadOnlyDictionary<string, double>? GetModifiers()
    {
        var boostComponent = cityMapState.CityWonder?.Components.OfType<BoostResourceComponent>().ToList();
        return boostComponent?.Count > 0
            ? boostComponent.ToDictionary(src => src.ResourceId!, src => src.GetValue(cityMapState.CityWonderLevel))
            : null;
    }

    public void UpdateStats(CityMapEntity target)
    {
        var building = cityMapState.Buildings[target.CityEntityId];
        switch (building.Type)
        {
            case BuildingType.Home:
            case BuildingType.Farm:
            case BuildingType.Barracks:
            case BuildingType.Workshop:
            case BuildingType.Aviary:
            case BuildingType.Quarry:
            case BuildingType.PapyrusField:
            case BuildingType.GoldMine:
            {
                UpdateHappiness(target);
                var modifiers = GetModifiers();
                if (modifiers == null)
                {
                    productionStatsProcessor.UpdateProduction(target);
                }
                else
                {
                    productionStatsProcessor.UpdateProduction(target, modifiers);
                }

                break;
            }
            case BuildingType.Collectable:
            case BuildingType.CultureSite:
            case BuildingType.RitualSite:
            {
                UpdateHappiness();
                UpdateProduction();
                break;
            }
            case BuildingType.CityHall:
            case BuildingType.Evolving:
            {
                UpdateEvolvingBuildings();
                UpdateHappiness();
                UpdateProduction();
                break;
            }
            case BuildingType.ExtractionPoint:
            case BuildingType.RiceFarm:
            case BuildingType.FishingPier:
            {
                var modifiers = GetModifiers();
                if (modifiers == null)
                {
                    productionStatsProcessor.UpdateProduction(target);
                }
                else
                {
                    productionStatsProcessor.UpdateProduction(target, modifiers);
                }
                break;
            }
        }

        cityMapState.CityStats = CityStatsProcessor.Update(cityMapState.CityMapEntities);
    }
}
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Core.CityPlanner;

public class CityStatsCalculator(
    ICityPlannerDataService cityPlannerDataService,
    ICityMapStateCoreFactory cityMapStateFactory,
    IMapAreaFactory mapAreaFactory,
    ICityStatsProcessorFactory cityStatsProcessorFactory) : ICityStatsCalculator
{
    public async Task<CityStats> Calculate(HohCity city)
    {
        var cityPlannerData = await cityPlannerDataService.GetCityPlannerDataAsync(city.InGameCityId);

        var cityMapState = cityMapStateFactory.Create(cityPlannerData.Buildings, 
            cityPlannerData.Ages, city,
            cityPlannerData.Wonders.FirstOrDefault(src => src.Id == city.WonderId));
        var mapArea = mapAreaFactory.Create(cityPlannerData.City.InitConfigs.Grid.ExpansionSize,
            cityPlannerData.Expansions, city.UnlockedExpansions,
            cityPlannerData.City.Components.OfType<CityCultureAreaComponent>());
        var lockedMapEntities = cityMapState.CityMapEntities.Values.Where(e => mapArea.IntersectsWithLocked(e.Bounds))
            .ToList();
        foreach (var lockedMapEntity in lockedMapEntities)
        {
            lockedMapEntity.ExcludeFromStats = true;
        }

        var statsProcessor = cityStatsProcessorFactory.Create(cityMapState,
            cityPlannerData.City.Components.OfType<CityCultureAreaComponent>());
        return statsProcessor.UpdateStats();
    }
}

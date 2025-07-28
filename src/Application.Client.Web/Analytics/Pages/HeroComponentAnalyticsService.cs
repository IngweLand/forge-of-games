using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Analytics.Pages;

public class HeroComponentAnalyticsService(IAnalyticsService analyticsService) : IHeroComponentAnalyticsService
{
    public void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object>? eventParams = null)
    {
        var allParameters = eventParams != null
            ? baseParams.Concat(eventParams).ToDictionary(x => x.Key, x => x.Value)
            : baseParams;

        _ = analyticsService.TrackEvent(eventName, allParameters);
    }

    public void TrackHeroLevelChange(IReadOnlyDictionary<string, object> baseParams,
        HeroProfileIdentifier profileIdentifier)
    {
        var eventParams = new Dictionary<string, object>
        {
            {AnalyticsParams.LEVEL, $"{profileIdentifier.Level}:{profileIdentifier.AscensionLevel}:" +
                                     $"{profileIdentifier.AbilityLevel}:{profileIdentifier.AwakeningLevel}:" +
                                     $"{profileIdentifier.BarracksLevel}"},
        };
        TrackEvent(AnalyticsEvents.SELECT_HERO_LEVEL, baseParams, eventParams);
    }
    
    public void TrackProgressionCalculatorEvent(IReadOnlyDictionary<string, object> baseParams,
        HeroLevelSpecs targetLevel)
    {
        var eventParams = new Dictionary<string, object>
        {
            {AnalyticsParams.LEVEL, $"{targetLevel.Level}:{targetLevel.AscensionLevel}"},
        };
        TrackEvent(AnalyticsEvents.PICK_HERO_TARGET_LEVEL, baseParams, eventParams);
    }
    
    public void TrackBattleNavigation(IReadOnlyDictionary<string, object> baseParams, UnitBattleViewModel battle)
    {
        var eventParams = new Dictionary<string, object>
        {
            {AnalyticsParams.UNIT_ID, battle.UnitId},
            {AnalyticsParams.BATTLE_DEFINITION_ID, battle.BattleDefinitionId},
        };
        TrackEvent(AnalyticsEvents.NAVIGATE_HERO_BATTLE, baseParams, eventParams);
    }
}

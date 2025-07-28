using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.Application.Client.Web.Analytics.Pages;

public class BattleLogPageAnalyticsService(
    IAnalyticsService analyticsService,
    IBattleDefinitionIdFactory battleDefinitionIdFactory) : IBattleLogPageAnalyticsService
{
    private static readonly string[] HeroParams =
    [
        AnalyticsParams.HERO1, AnalyticsParams.HERO2, AnalyticsParams.HERO3, AnalyticsParams.HERO4,
        AnalyticsParams.HERO5,
    ];

    public void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object>? eventParams = null)
    {
        var allParameters = eventParams != null
            ? baseParams.Concat(eventParams).ToDictionary(x => x.Key, x => x.Value)
            : baseParams;

        _ = analyticsService.TrackEvent(eventName, allParameters);
    }

    public void TrackFormChange(BattleSearchRequest request, IReadOnlyDictionary<string, object> baseParams)
    {
        var defId = battleDefinitionIdFactory.Create(request);
        var eventParams = new Dictionary<string, object>
        {
            {AnalyticsParams.BATTLE_DEFINITION_ID, defId},
            {AnalyticsParams.BATTLE_TYPE, request.BattleType.ToString()},
        };

        var unitIds = request.UnitIds.ToList();

        for (var i = 0; i < Math.Min(unitIds.Count, HeroParams.Length); i++)
        {
            eventParams.Add(HeroParams[i], unitIds[i]);
        }

        eventParams.Add(AnalyticsParams.HEROES,
            string.Join(':', unitIds.Select(x => HohStringParser.GetConcreteId2(x, '_'))));

        TrackEvent(AnalyticsEvents.BATTLE_LOG_FORM_SUBMIT, baseParams, eventParams);
    }

    public void TrackSquadProfileView(string eventName, IReadOnlyDictionary<string, object> baseParams, string unitId)
    {
        var eventParams = new Dictionary<string, object>
        {
            {AnalyticsParams.UNIT_ID, unitId},
        };
        TrackEvent(eventName, baseParams, eventParams);
    }
}

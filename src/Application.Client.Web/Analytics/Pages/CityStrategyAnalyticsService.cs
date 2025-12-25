using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Analytics.Pages;

public class CityStrategyAnalyticsService(IAnalyticsService analyticsService) : ICityStrategyAnalyticsService
{
    private readonly IReadOnlyDictionary<string, object> _emptyParams = new Dictionary<string, object>();

    public void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object> eventParams)
    {
        var allParameters = baseParams.Concat(eventParams).ToDictionary(x => x.Key, x => x.Value);

        _ = analyticsService.TrackEvent(eventName, allParameters);
    }

    public void TrackCityStrategyOpening(string strategyId, CityId inGameCityId, WonderId wonderId)
    {
        var eventParams = new Dictionary<string, object>
        {
            {AnalyticsParams.CITY_ID, inGameCityId.ToString()},
            {AnalyticsParams.CITY_STRATEGY_ID, strategyId},
        };

        if (wonderId != WonderId.Undefined)
        {
            eventParams.Add(AnalyticsParams.WONDER_ID, wonderId.ToString());
        }

        _ = analyticsService.TrackEvent(AnalyticsEvents.OPEN_CITY_STRATEGY, eventParams);
    }

    public void TrackEvent(string eventName, IReadOnlyDictionary<string, object> eventParams)
    {
        _ = analyticsService.TrackEvent(eventName, eventParams);
    }

    public void TrackEvent(string eventName)
    {
        _ = analyticsService.TrackEvent(eventName, _emptyParams);
    }

    public void TrackCityStrategyCreation(NewCityRequest request)
    {
        var eventParams = new Dictionary<string, object>
        {
            {AnalyticsParams.CITY_ID, request.CityId.ToString()},
        };

        if (request.WonderId != WonderId.Undefined)
        {
            eventParams.Add(AnalyticsParams.WONDER_ID, request.WonderId.ToString());
        }

        _ = analyticsService.TrackEvent(AnalyticsEvents.CREATE_CITY_STRATEGY, eventParams);
    }
}

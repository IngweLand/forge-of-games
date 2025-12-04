using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Analytics.Pages;

public class CityPlannerAnalyticsService(IAnalyticsService analyticsService) : ICityPlannerAnalyticsService
{
    private readonly IReadOnlyDictionary<string, object> _emptyParams = new Dictionary<string, object>();

    public void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object> eventParams)
    {
        var allParameters = baseParams.Concat(eventParams).ToDictionary(x => x.Key, x => x.Value);

        _ = analyticsService.TrackEvent(eventName, allParameters);
    }
    
    public void TrackEvent(string eventName, IReadOnlyDictionary<string, object> eventParams)
    {
        _ = analyticsService.TrackEvent(eventName, eventParams);
    }

    public void TrackEvent(string eventName)
    {
        _ = analyticsService.TrackEvent(eventName, _emptyParams);
    }
    
    public void TrackCityCreation(NewCityRequest request)
    {
        var eventParams = new Dictionary<string, object>
        {
            {AnalyticsParams.CITY_ID, request.CityId.ToString()},
        };

        if (request.WonderId != WonderId.Undefined)
        {
            eventParams.Add(AnalyticsParams.WONDER_ID, request.WonderId.ToString());
        }

        _ = analyticsService.TrackEvent(AnalyticsEvents.CP_CREATE_CITY, eventParams);
    }

    public void TrackCityOpening(string layoutId, CityId inGameCityId, WonderId wonderId)
    {
        var eventParams = new Dictionary<string, object>
        {
            {AnalyticsParams.CITY_ID, inGameCityId.ToString()},
            {AnalyticsParams.CITY_PLANNER_LAYOUT_ID, layoutId},
        };

        if (wonderId != WonderId.Undefined)
        {
            eventParams.Add(AnalyticsParams.WONDER_ID, wonderId.ToString());
        }

        _ = analyticsService.TrackEvent(AnalyticsEvents.CP_OPEN_CITY, eventParams);
    }
}

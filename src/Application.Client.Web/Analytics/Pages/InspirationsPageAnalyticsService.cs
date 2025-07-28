using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;

namespace Ingweland.Fog.Application.Client.Web.Analytics.Pages;

public class InspirationsPageAnalyticsService(IAnalyticsService analyticsService) : IInspirationsPageAnalyticsService
{
    public void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object>? eventParams = null)
    {
        var allParameters = eventParams != null
            ? baseParams.Concat(eventParams).ToDictionary(x => x.Key, x => x.Value)
            : baseParams;

        _ = analyticsService.TrackEvent(eventName, allParameters);
    }

    public void TrackSearch(CityInspirationsSearchRequest request, IReadOnlyDictionary<string, object> baseParams)
    {
        var eventParams = new Dictionary<string, object>
        {
            {AnalyticsParams.CITY_ID, request.CityId.ToString()},
            {AnalyticsParams.AGE_ID, request.AgeId},
            {AnalyticsParams.SEARCH_PREFERENCE, request.SearchPreference.ToString()},
            {AnalyticsParams.PREMIUM, request.AllowPremiumEntities},
            {AnalyticsParams.EXPANSIONS, request.OpenedExpansionsHash != null},
        };

        TrackEvent(AnalyticsEvents.INSPIRATIONS_FORM_SUBMIT, baseParams, eventParams);
    }
}

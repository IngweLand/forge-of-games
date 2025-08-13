using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.Analytics.Pages;

public class AlliancePageAnalyticsService(IAnalyticsService analyticsService) : IAlliancePageAnalyticsService
{
    public void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object>? eventParams = null)
    {
        var allParameters = eventParams != null
            ? baseParams.Concat(eventParams).ToDictionary(x => x.Key, x => x.Value)
            : baseParams;

        _ = analyticsService.TrackEvent(eventName, allParameters);
    }

    public void TrackChartView(string eventName, IReadOnlyDictionary<string, object> baseParams,
        string source, bool isExpanded)
    {
        var eventParams = new Dictionary<string, object>
        {
            {AnalyticsParams.SOURCE, source},
            {AnalyticsParams.STATE, isExpanded ? AnalyticsParams.Values.States.ON : AnalyticsParams.Values.States.OFF},
        };
        TrackEvent(eventName, baseParams, eventParams);
    }
}

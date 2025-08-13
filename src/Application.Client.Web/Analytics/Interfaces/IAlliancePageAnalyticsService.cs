namespace Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;

public interface IAlliancePageAnalyticsService
{
    void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object>? eventParams = null);
    void TrackChartView(string eventName, IReadOnlyDictionary<string, object> baseParams,
        string source, bool isExpanded);
}

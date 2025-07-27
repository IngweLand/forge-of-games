namespace Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;

public interface IPlayerProfilePageAnalyticsService
{
    void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object>? eventParams = null);

    void TrackChartView(string eventName, IReadOnlyDictionary<string, object> baseParams,
        string source, bool isExpanded);

    void TrackAllianceNavigation(string eventName, IReadOnlyDictionary<string, object> baseParams,
        string source, int allianceId);

    void TrackSquadProfileView(string eventName, IReadOnlyDictionary<string, object> baseParams,
        string source, string unitId);
}

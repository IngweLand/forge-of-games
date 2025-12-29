namespace Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;

public interface IEquipmentConfiguratorAnalyticsService
{
    void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object> eventParams);

    void TrackEvent(string eventName);
    void TrackOpening(string profileId);

    void TrackEvent(string eventName, IReadOnlyDictionary<string, object> eventParams);
}

using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.Analytics.Pages;

public class EquipmentConfiguratorAnalyticsService(IAnalyticsService analyticsService)
    : IEquipmentConfiguratorAnalyticsService
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

    public void TrackOpening(string profileId)
    {
        var eventParams = new Dictionary<string, object>
        {
            {AnalyticsParams.EQUIPMENT_PROFILE_ID, profileId},
        };

        _ = analyticsService.TrackEvent(AnalyticsEvents.OPEN_EQUIPMENT_CONFIGURATOR, eventParams);
    }
}

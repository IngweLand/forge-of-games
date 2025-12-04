using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;

public interface ICityPlannerAnalyticsService
{
    void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object> eventParams);

    void TrackEvent(string eventName);
    void TrackCityCreation(NewCityRequest request);
    void TrackCityOpening(string layoutId, CityId inGameCityId, WonderId wonderId);

    void TrackEvent(string eventName, IReadOnlyDictionary<string, object> eventParams);
}

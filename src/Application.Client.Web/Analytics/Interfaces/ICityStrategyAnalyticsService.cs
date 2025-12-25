using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;

public interface ICityStrategyAnalyticsService
{
    void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object> eventParams);

    void TrackEvent(string eventName);
    void TrackCityStrategyCreation(NewCityRequest request);
    void TrackCityStrategyOpening(string strategyId, CityId inGameCityId, WonderId wonderId);

    void TrackEvent(string eventName, IReadOnlyDictionary<string, object> eventParams);
}

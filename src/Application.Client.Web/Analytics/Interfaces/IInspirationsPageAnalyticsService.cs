using Ingweland.Fog.Dtos.Hoh.PlayerCity;

namespace Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;

public interface IInspirationsPageAnalyticsService
{
    void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object>? eventParams = null);

    void TrackSearch(CityInspirationsSearchRequest request, IReadOnlyDictionary<string, object> baseParams);
}

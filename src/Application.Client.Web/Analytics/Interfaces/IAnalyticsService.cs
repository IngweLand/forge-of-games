namespace Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;

public interface IAnalyticsService
{
    Task TrackEvent(string eventName, IReadOnlyDictionary<string, object> eventParams);
}

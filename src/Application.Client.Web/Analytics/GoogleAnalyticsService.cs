using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.Analytics;

public class GoogleAnalyticsService(IJSInteropService jsInteropService, ILogger<GoogleAnalyticsService> logger)
    : IAnalyticsService
{
    public async Task TrackEvent(string eventName, IReadOnlyDictionary<string, object> eventParams)
    {
        if (!OperatingSystem.IsBrowser())
        {
            logger.LogWarning("Google analytics is only supported in the browser.");
            return;
        }

        try
        {
            await jsInteropService.SendToGtag("event", eventName, eventParams);
            logger.LogDebug("GA event sent: {EventName}", eventName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send GA event: {EventName}", eventName);
        }
    }
}

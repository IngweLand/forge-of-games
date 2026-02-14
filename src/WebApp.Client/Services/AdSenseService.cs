using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Microsoft.JSInterop;

namespace Ingweland.Fog.WebApp.Client.Services;

public class AdSenseService(IJSRuntime jsRuntime) :IAdSenseService
{
    public ValueTask InitializeAd(string adSlotId)
    {
        return jsRuntime.InvokeVoidAsync("Fog.Webapp.AdSense.initializeAd", adSlotId);
    }

    public ValueTask ClearAd(string adSlotId)
    {
        return jsRuntime.InvokeVoidAsync("Fog.Webapp.AdSense.clearAd", adSlotId);
    }
}

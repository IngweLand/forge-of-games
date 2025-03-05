using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Microsoft.JSInterop;

namespace Ingweland.Fog.WebApp.Client.Services;

public class JSInteropService(IJSRuntime jsRuntime) : IJSInteropService
{
    public ValueTask ResetScrollPositionAsync()
    {
        return jsRuntime.InvokeVoidAsync("Fog.Webapp.Common.resetScrollPosition");
    }

    public ValueTask HideLoadingIndicatorAsync()
    {
        return jsRuntime.InvokeVoidAsync("Fog.Webapp.Common.hidePageLoadingIndicator");
    }
    
    public ValueTask ShowLoadingIndicatorAsync()
    {
        return jsRuntime.InvokeVoidAsync("Fog.Webapp.Common.showPageLoadingIndicator");
    }

    public ValueTask<bool> CopyToClipboardAsync(string payload)
    {
        return jsRuntime.InvokeAsync<bool>("Fog.Webapp.Common.copyToClipboard", payload);
    }

    public ValueTask<bool> IsMobileAsync()
    {
        return jsRuntime.InvokeAsync<bool>("Fog.Webapp.Common.isMobile");
    }

    public ValueTask OpenUrlAsync(string url, string target)
    {
        return jsRuntime.InvokeVoidAsync("open", url, target);
    }
}

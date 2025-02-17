using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Microsoft.JSInterop;

namespace Ingweland.Fog.WebApp.Client.Services;

public class JSInteropService(IJSRuntime jsRuntime) : IJSInteropService
{
    public ValueTask ResetScrollPositionAsync()
    {
        return jsRuntime.InvokeVoidAsync("Fog.Webapp.Common.resetScrollPosition");
    }

    public ValueTask RemoveLoadingIndicatorAsync()
    {
        return jsRuntime.InvokeVoidAsync("Fog.Webapp.Common.removeLoadingIndicator");
    }

    public ValueTask<bool> CopyToClipboardAsync(string payload)
    {
        return jsRuntime.InvokeAsync<bool>("Fog.Webapp.Common.copyToClipboard", payload);
    }
}

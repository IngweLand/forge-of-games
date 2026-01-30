using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Microsoft.AspNetCore.Components;
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

    public ValueTask ScrollTo(ElementReference target, int position, bool smooth = false)
    {
        return jsRuntime.InvokeVoidAsync("Fog.Webapp.Common.scrollTo", target, position, smooth);
    }

    public ValueTask ScrollToBottomAsync(ElementReference target, bool smooth = false)
    {
        return jsRuntime.InvokeVoidAsync("Fog.Webapp.Common.scrollToBottom", target, smooth);
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

    public ValueTask SendToGtag(string command, string target, IReadOnlyDictionary<string, object> parameters)
    {
        return jsRuntime.InvokeVoidAsync("Fog.Webapp.Analytics.gtag", command, target, parameters);
    }

    public ValueTask SaveFileAsync(string fileName, string contentType, object content)
    {
        return jsRuntime.InvokeVoidAsync("Fog.Webapp.Common.saveFile", fileName, contentType, content);
    }
}

using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Services.Abstractions;

public interface IJSInteropService
{
    ValueTask<bool> CopyToClipboardAsync(string payload);
    ValueTask<bool> IsMobileAsync();
    ValueTask OpenUrlAsync(string url, string target);
    ValueTask HideLoadingIndicatorAsync();
    ValueTask ResetScrollPositionAsync();
    ValueTask ShowLoadingIndicatorAsync();
    ValueTask ScrollTo(ElementReference target, int position, bool smooth = false);
}

using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface IJSInteropService
{
    ValueTask<bool> CopyToClipboardAsync(string payload);
    ValueTask<bool> IsMobileAsync();
    ValueTask OpenUrlAsync(string url, string target);
    ValueTask HideLoadingIndicatorAsync();
    ValueTask ResetScrollPositionAsync();
    ValueTask ShowLoadingIndicatorAsync();
    ValueTask ScrollTo(ElementReference target, int position, bool smooth = false);
    ValueTask ScrollToBottomAsync(ElementReference target, bool smooth = false);
    ValueTask SendToGtag(string command, string target, IReadOnlyDictionary<string, object> parameters);
    ValueTask SaveFileAsync(string fileName, string contentType, object content);
}

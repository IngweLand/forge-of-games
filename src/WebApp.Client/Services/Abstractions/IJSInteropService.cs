namespace Ingweland.Fog.WebApp.Client.Services.Abstractions;

public interface IJSInteropService
{
    ValueTask<bool> CopyToClipboardAsync(string payload);
    ValueTask<bool> IsMobileAsync();
    ValueTask OpenUrlAsync(string url, string target);
    ValueTask RemoveLoadingIndicatorAsync();
    ValueTask ResetScrollPositionAsync();
}

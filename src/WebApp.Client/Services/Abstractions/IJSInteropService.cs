namespace Ingweland.Fog.WebApp.Client.Services.Abstractions;

public interface IJSInteropService
{
    ValueTask ResetScrollPositionAsync();
    ValueTask RemoveLoadingIndicatorAsync();
    ValueTask<bool> CopyToClipboardAsync(string payload);
}

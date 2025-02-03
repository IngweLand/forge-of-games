using Microsoft.JSInterop;

namespace Ingweland.Fog.WebApp.Client.Services;

public sealed class ClipboardService(IJSRuntime jsRuntime) : IClipboardService
{
    public ValueTask<string> ReadTextAsync()
    {
        return jsRuntime.InvokeAsync<string>("navigator.clipboard.readText");
    }

    public ValueTask WriteTextAsync(string text)
    {
        return jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
    }
}

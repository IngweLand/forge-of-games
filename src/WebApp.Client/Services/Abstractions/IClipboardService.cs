namespace Ingweland.Fog.WebApp.Client.Services.Abstractions;

public interface IClipboardService
{
    ValueTask<string> ReadTextAsync();
    ValueTask WriteTextAsync(string text);
}

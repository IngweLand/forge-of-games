namespace Ingweland.Fog.WebApp.Client.Services;

public interface IClipboardService
{
    ValueTask<string> ReadTextAsync();
    ValueTask WriteTextAsync(string text);
}

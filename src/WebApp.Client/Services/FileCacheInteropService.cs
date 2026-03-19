using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Microsoft.JSInterop;

namespace Ingweland.Fog.WebApp.Client.Services;

public class FileCacheInteropService(IJSRuntime jsRuntime) : IFileCacheInteropService
{
    private const string VERSION_KEY = "version";

    public async Task<byte[]?> LoadAsync(string key) =>
        await jsRuntime.InvokeAsync<byte[]?>("Fog.Webapp.FileCache.loadFile", key);

    public async Task SaveAsync(string key, byte[] data) =>
        await jsRuntime.InvokeVoidAsync("Fog.Webapp.FileCache.saveFile", key, data);

    public async Task DeleteAsync(string key) =>
        await jsRuntime.InvokeVoidAsync("Fog.Webapp.FileCache.deleteFile", key);

    public async Task<IReadOnlyCollection<string>> ListKeysAsync(string key) =>
        await jsRuntime.InvokeAsync<IReadOnlyCollection<string>>("Fog.Webapp.FileCache.listKeys", key);

    public async Task SaveVersionAsync(string version) =>
        await jsRuntime.InvokeVoidAsync("Fog.Webapp.FileCache.saveString", VERSION_KEY, version);

    public async Task<string?> GetVersionAsync() =>
        await jsRuntime.InvokeAsync<string?>("Fog.Webapp.FileCache.getString", VERSION_KEY);
}

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface IFileCacheInteropService
{
    Task<byte[]?> LoadAsync(string key);
    Task SaveAsync(string key, byte[] data);
    Task DeleteAsync(string key);
    Task<IReadOnlyCollection<string>> ListKeysAsync(string key);
    Task SaveVersionAsync(string version);

    Task<string?> GetVersionAsync();
}

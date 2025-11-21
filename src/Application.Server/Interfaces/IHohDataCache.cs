namespace Ingweland.Fog.Application.Server.Interfaces;

public interface IHohDataCache
{
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, Guid dataVersion);
    T GetOrAdd<T>(string key, Func<T> addItemFactory, Guid dataVersion);
}

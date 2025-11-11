namespace Ingweland.Fog.Application.Server.Interfaces;

public interface IHohDataCache
{
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, Guid dataVersion);
}

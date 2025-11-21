using Ingweland.Fog.Application.Server.Interfaces;
using LazyCache;

namespace Ingweland.Fog.Application.Server.Caching;

public class HohDataCache(IAppCache appCache) : IHohDataCache
{
    private readonly HashSet<string> _keys = [];
    private Guid _dataVersion = Guid.NewGuid();

    public Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, Guid dataVersion)
    {
        ClearCacheIfRequired(dataVersion);
        _keys.Add(key);
        return appCache.GetOrAddAsync(key, addItemFactory, DateTimeOffset.MaxValue);
    }

    public T GetOrAdd<T>(string key, Func<T> addItemFactory, Guid dataVersion)
    {
        ClearCacheIfRequired(dataVersion);
        _keys.Add(key);
        return appCache.GetOrAdd(key, addItemFactory, DateTimeOffset.MaxValue);
    }

    private void ClearCacheIfRequired(Guid version)
    {
        if (version == _dataVersion)
        {
            return;
        }

        foreach (var key in _keys)
        {
            appCache.Remove(key);
        }

        _keys.Clear();
        _dataVersion = version;
    }
}

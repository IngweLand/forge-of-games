using Ingweland.Fog.Application.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Ingweland.Fog.Application.Client.Web.Caching;

[Obsolete(
    "This type was implmented during the migration to browsel local caching, but it should be removed in the future.")]
public class HohDataCache(IMemoryCache cache) : IHohDataCache
{
    private readonly HashSet<string> _keys = [];
    private Guid _dataVersion = Guid.NewGuid();

    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, Guid dataVersion)
    {
        ClearCacheIfRequired(dataVersion);

        if (cache.TryGetValue(key, out T? cachedResult) && cachedResult != null)
        {
            return cachedResult;
        }

        var result = await addItemFactory();
        _keys.Add(key);
        cache.Set(key, result, DateTimeOffset.MaxValue);
        return result;
    }

    public T GetOrAdd<T>(string key, Func<T> addItemFactory, Guid dataVersion)
    {
        ClearCacheIfRequired(dataVersion);

        if (cache.TryGetValue(key, out T? cachedResult) && cachedResult != null)
        {
            return cachedResult;
        }

        var result = addItemFactory();
        _keys.Add(key);
        cache.Set(key, result, DateTimeOffset.MaxValue);
        return result;
    }

    public void Clear()
    {
        foreach (var key in _keys)
        {
            cache.Remove(key);
        }

        _keys.Clear();
    }

    private void ClearCacheIfRequired(Guid version)
    {
        if (version == _dataVersion)
        {
            return;
        }

        Clear();

        _dataVersion = version;
    }
}

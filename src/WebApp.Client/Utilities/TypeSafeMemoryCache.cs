using System.Collections.Concurrent;

namespace Ingweland.Fog.WebApp.Client.Utilities;

public class TypeSafeMemoryCache : ITypeSafeMemoryCache
{
    private readonly ConcurrentDictionary<(object Key, Type Type), object> _cache = new();

    public void Set<T>(object key, T value)
    {
        if (value == null)
        {
            return;
        }
        
        _cache[(key, typeof(T))] = value;
    }

    public bool TryGetValue<T>(object key, out T? value)
    {
        if (_cache.TryGetValue((key, typeof(T)), out var cachedValue))
        {
            value = (T?)cachedValue;
            return true;
        }

        value = default;
        return false;
    }

    public void Remove<T>(object key)
    {
        _cache.TryRemove((key, typeof(T)), out _);
    }

    public void Clear()
    {
        _cache.Clear();
    }
}

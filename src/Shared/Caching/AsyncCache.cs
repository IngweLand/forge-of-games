using System.Collections.Concurrent;

namespace Ingweland.Fog.Shared.Caching;

public class AsyncCache<TKey, TValue>(Func<TKey, Task<TValue?>> factory)
    where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, TValue> _cache = new();
    private readonly Func<TKey, Task<TValue?>> _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    private readonly ConcurrentDictionary<TKey, SemaphoreSlim> _semaphores = new();
    public ICollection<TKey> Keys => _cache.Keys;
    public ICollection<TValue> Values => _cache.Values;

    public IReadOnlyDictionary<TKey, TValue> GetAll()
    {
        return new Dictionary<TKey, TValue>(_cache);
    }

    public async Task<TValue?> GetAsync(TKey key)
    {
        if (_cache.TryGetValue(key, out var cachedValue))
        {
            return cachedValue;
        }

        var semaphore = _semaphores.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

        await semaphore.WaitAsync();
        try
        {
            if (_cache.TryGetValue(key, out cachedValue))
            {
                return cachedValue;
            }

            var value = await _factory(key);
            if (value == null)
            {
                return default;
            }

            _cache.TryAdd(key, value);
            return value;
        }
        finally
        {
            semaphore.Release();
        }
    }

    public async Task AddAsync(TKey key, TValue value)
    {
        var semaphore = _semaphores.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync();
        try
        {
            _cache.TryAdd(key, value);
        }
        finally
        {
            semaphore.Release();
        }
    }

    public void Remove(TKey key)
    {
        _cache.TryRemove(key, out _);
        if (_semaphores.TryRemove(key, out var semaphore))
        {
            semaphore.Dispose();
        }
    }

    public void Clear()
    {
        _cache.Clear();
        foreach (var semaphore in _semaphores.Values)
        {
            semaphore.Dispose();
        }

        _semaphores.Clear();
    }
}

using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Ingweland.Fog.Application.Server.PlayerCity;

public class FailedPlayerCityFetchesCache : IFailedPlayerCityFetchesCache
{
    private const string CacheKeyPrefix = "FailedPlayerCityFetch:";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);
    private readonly IMemoryCache _cache;

    public FailedPlayerCityFetchesCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    private static string CreateCacheKey(PlayerKey key)
    {
        return $"{CacheKeyPrefix}{key.WorldId}:{key.InGamePlayerId}";
    }

    public bool IsFailedFetch(PlayerKey key)
    {
        return _cache.TryGetValue(CreateCacheKey(key), out _);
    }

    public void AddFailedFetch(PlayerKey key)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(CacheDuration)
            .SetSlidingExpiration(TimeSpan.FromHours(1));

        _cache.Set(CreateCacheKey(key), true, cacheEntryOptions);
    }

    public void RemoveFailedFetchAsync(PlayerKey key)
    {
        _cache.Remove(CreateCacheKey(key));
    }
}

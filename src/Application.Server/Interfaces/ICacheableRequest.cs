namespace Ingweland.Fog.Application.Server.Interfaces;

public interface ICacheableRequest
{
    string CacheKey { get; }
    TimeSpan? Duration { get; } 
    DateTimeOffset? Expiration { get; }

    public DateTimeOffset GetExpiration()
    {
        return Expiration ?? DateTimeOffset.Now.Add(Duration ?? TimeSpan.FromMinutes(5));
    }
}

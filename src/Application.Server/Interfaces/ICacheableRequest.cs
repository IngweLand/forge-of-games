namespace Ingweland.Fog.Application.Server.Interfaces;

public interface ICacheableRequest
{
    TimeSpan? Duration { get; } 
    DateTimeOffset? Expiration { get; }

    public DateTimeOffset GetExpiration()
    {
        return Expiration ?? DateTimeOffset.Now.Add(Duration ?? TimeSpan.FromMinutes(5));
    }
}

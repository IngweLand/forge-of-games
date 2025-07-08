using Ingweland.Fog.Application.Server.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Behaviors;

public class CachingBehavior<TRequest, TResponse>(
    IMemoryCache cache,
    ILogger<CachingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableRequest
    where TResponse : class?
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (cache.TryGetValue(request.CacheKey, out var cached) && cached is TResponse response)
        {
            logger.LogDebug("Cache HIT for key: {CacheKey}", request.CacheKey);
            return response;
        }

        logger.LogInformation("Cache MISS for key: {CacheKey}", request.CacheKey);
        response = await next();

        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = response != null ? request.GetExpiration() : DateTimeOffset.UtcNow.AddHours(1),
        };

        cache.Set(request.CacheKey, response, cacheEntryOptions);

        logger.LogInformation("Cached response for key: {CacheKey} with expiration: {Expiration}. Is null: {IsNUll}",
            request.CacheKey, cacheEntryOptions.AbsoluteExpiration, response == null);

        return response;
    }
}

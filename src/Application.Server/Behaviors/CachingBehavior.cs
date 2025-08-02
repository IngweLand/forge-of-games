using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using LazyCache;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Behaviors;

public class CachingBehavior<TRequest, TResponse>(
    IAppCache appCache,
    ICacheKeyFactory cacheKeyFactory,
    ILogger<CachingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableRequest
    where TResponse : class?
{
    public Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var cacheKey = cacheKeyFactory.CreateKey(request);
        return appCache.GetOrAddAsync(cacheKey, async entry =>
        {
            logger.LogInformation("Cache MISS for key: {CacheKey}", cacheKey);
            
            var response = await next();

            var absoluteExpiration = response != null ? request.GetExpiration() : DateTimeOffset.UtcNow.AddMinutes(5);
            
            logger.LogInformation(
                "Cached response for key: {CacheKey} with expiration: {Expiration}. Is null: {IsNUll}",
                cacheKey, absoluteExpiration, response == null);

            entry.AbsoluteExpiration = absoluteExpiration;
            return response;
        });
    }
}

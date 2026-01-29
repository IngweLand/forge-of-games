using FluentResults;
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
            DateTimeOffset absoluteExpiration;
            if (response == null)
            {
                absoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5);
                logger.LogInformation("Cached null response for key: {CacheKey} with expiration: {Expiration}.",
                    cacheKey, absoluteExpiration);
            }
            else if (response is IResultBase r)
            {
                absoluteExpiration = r.IsSuccess ? request.GetExpiration() : DateTimeOffset.UtcNow.AddMinutes(1);
                logger.LogInformation(
                    "Cached Result response for key: {CacheKey} with expiration: {Expiration}. Is success: {isSuccess}",
                    cacheKey, absoluteExpiration, r.IsSuccess);
            }
            else
            {
                absoluteExpiration = request.GetExpiration();
                logger.LogInformation("Cached response for key: {CacheKey} with expiration: {Expiration}.", cacheKey,
                    absoluteExpiration);
            }

            entry.AbsoluteExpiration = absoluteExpiration;
            return response;
        });
    }
}

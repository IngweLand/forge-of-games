using Ingweland.Fog.Application.Core.Repository.Abstractions;
using Ingweland.Fog.Application.Server.Caching;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using LazyCache;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.Application.Server.Services.Queries;

// ReSharper disable once ClassNeverInstantiated.Global
public record GetHohCoreDataQuery() : IRequest<VersionedResponse<byte[]?>>;

public class GetHohCoreDataQueryHandler(
    IHohDataProvider dataProvider,
    IProtobufSerializer protobufSerializer,
    IAppCache appCache,
    IOptionsSnapshot<ResourceSettings> resourceSettings,
    ICacheKeyFactory cacheKeyFactory,
    ILogger<GetHohCoreDataQueryHandler> logger)
    : IRequestHandler<GetHohCoreDataQuery, VersionedResponse<byte[]?>>
{
    public async Task<VersionedResponse<byte[]?>> Handle(GetHohCoreDataQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = cacheKeyFactory.HohCoreData;
        var cached = await appCache.GetOrAddAsync(cacheKey, async entry =>
        {
            logger.LogInformation("Cache MISS for key: {CacheKey}", cacheKey);

            var data = protobufSerializer.SerializeToBytes(await dataProvider.GetDataAsync());
            var absoluteExpiration = DateTimeOffset.MaxValue;
            logger.LogInformation("Cached response for key: {CacheKey} with expiration: {Expiration}.", cacheKey,
                absoluteExpiration);

            entry.AbsoluteExpiration = absoluteExpiration;
            return new VersionedCachedResource<byte[]?>(resourceSettings.Value.HohCoreDataVersion, data);
        });

        if (cached is null)
        {
            return new VersionedResponse<byte[]?>(string.Empty, null);
        }

        return new VersionedResponse<byte[]?>(cached.Version, cached.Data);
    }
}

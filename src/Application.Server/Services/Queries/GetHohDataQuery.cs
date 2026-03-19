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

public record GetHohDataQuery(string? Version) : IRequest<VersionedResponse<byte[]?>>;

public class GetHohDataQueryHandler(
    IHohDataProvider dataProvider,
    IProtobufSerializer protobufSerializer,
    IAppCache appCache,
    IOptionsSnapshot<ResourceSettings> resourceSettings,
    ICacheKeyFactory cacheKeyFactory,
    ILogger<GetHohDataQueryHandler> logger)
    : IRequestHandler<GetHohDataQuery, VersionedResponse<byte[]?>>
{
    public async Task<VersionedResponse<byte[]?>> Handle(GetHohDataQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = cacheKeyFactory.HohData;
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
            return new VersionedResponse<byte[]?>(true, string.Empty, null);
        }

        if (request.Version == cached.Version)
        {
            return new VersionedResponse<byte[]?>(true, cached.Version, null);
        }

        return new VersionedResponse<byte[]?>(false, cached.Version, cached.Data);
    }
}

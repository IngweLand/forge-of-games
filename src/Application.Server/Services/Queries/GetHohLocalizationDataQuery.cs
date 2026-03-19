using System.Globalization;
using Ingweland.Fog.Application.Core.Repository.Abstractions;
using Ingweland.Fog.Application.Server.Caching;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Ingweland.Fog.Shared.Localization;
using LazyCache;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.Application.Server.Services.Queries;

public record GetHohLocalizationDataQuery(string? Version) : IRequest<VersionedResponse<byte[]?>>;

public class GetHohLocalizationDataQueryHandler(
    IHohLocalizationDataProvider dataProvider,
    IProtobufSerializer protobufSerializer,
    IAppCache appCache,
    IOptionsSnapshot<ResourceSettings> resourceSettings,
    ICacheKeyFactory cacheKeyFactory,
    ILogger<GetHohLocalizationDataQueryHandler> logger)
    : IRequestHandler<GetHohLocalizationDataQuery, VersionedResponse<byte[]?>>
{
    public Task<VersionedResponse<byte[]?>> Handle(GetHohLocalizationDataQuery request,
        CancellationToken cancellationToken)
    {
        var cultureCode = CultureInfo.CurrentCulture.Name;
        var cacheKey = cacheKeyFactory.HohLocalizationData(cultureCode);
        var cached = appCache.GetOrAdd(cacheKey, entry =>
        {
            logger.LogInformation("Cache MISS for key: {CacheKey}", cacheKey);
            var data = dataProvider.GetData();
            var concreteData = data.TryGetValue(cultureCode, out var localizationData)
                ? localizationData
                : data[HohSupportedCultures.DefaultCulture];
            var absoluteExpiration = DateTimeOffset.MaxValue;
            logger.LogInformation("Cached response for key: {CacheKey} with expiration: {Expiration}.", cacheKey,
                absoluteExpiration);

            entry.AbsoluteExpiration = absoluteExpiration;
            return new VersionedCachedResource<byte[]?>(resourceSettings.Value.HohCoreDataVersion,
                protobufSerializer.SerializeToBytes(concreteData));
        });

        if (cached is null)
        {
            return Task.FromResult(new VersionedResponse<byte[]?>(true, string.Empty, null));
        }

        if (request.Version == cached.Version)
        {
            return Task.FromResult(new VersionedResponse<byte[]?>(true, cached.Version, null));
        }

        return Task.FromResult(new VersionedResponse<byte[]?>(false, cached.Version, cached.Data));
    }
}

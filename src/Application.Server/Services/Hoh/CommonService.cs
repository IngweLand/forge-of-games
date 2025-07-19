using AutoMapper;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh;
using LazyCache;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class CommonService(
    IHohCoreDataRepository hohCoreDataRepository,
    IResourceDtoFactory resourceDtoFactory,
    IMapper mapper,
    IAppCache appCache,
    ICacheKeyFactory cacheKeyFactory) : ICommonService
{
    public Task<IReadOnlyCollection<AgeDto>> GetAgesAsync()
    {
        return appCache.GetOrAddAsync(cacheKeyFactory.HohAges(), async () =>
            {
                var ages = await hohCoreDataRepository.GetAges();
                return mapper.Map<IReadOnlyCollection<AgeDto>>(ages);
            },
            DateTimeOffset.Now.Add(FogConstants.DefaultHohDataEntityCacheTime));
    }

    public Task<IReadOnlyCollection<ResourceDto>> GetResourceAsync()
    {
        return appCache.GetOrAddAsync(cacheKeyFactory.HohResources(), async () =>
            {
                var resources = await hohCoreDataRepository.GetResources();
                return (IReadOnlyCollection<ResourceDto>) resources.Select(x => resourceDtoFactory.Create(x, x.Age))
                    .ToList();
            },
            DateTimeOffset.Now.Add(FogConstants.DefaultHohDataEntityCacheTime));
    }
}

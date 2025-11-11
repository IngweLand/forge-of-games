using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class CommonService(
    IHohCoreDataRepository hohCoreDataRepository,
    IResourceDtoFactory resourceDtoFactory,
    IMapper mapper,
    IHohDataCache dataCache,
    ICacheKeyFactory cacheKeyFactory) : ICommonService
{
    public Task<IReadOnlyCollection<AgeDto>> GetAgesAsync()
    {
        var version = hohCoreDataRepository.Version;
        return dataCache.GetOrAddAsync(cacheKeyFactory.HohAges(version), async () =>
            {
                var ages = await hohCoreDataRepository.GetAges();
                return mapper.Map<IReadOnlyCollection<AgeDto>>(ages);
            },
            version);
    }

    public Task<IReadOnlyCollection<ResourceDto>> GetResourcesAsync()
    {
        var version = hohCoreDataRepository.Version;
        return dataCache.GetOrAddAsync(cacheKeyFactory.HohResources(version), async () =>
            {
                var resources = await hohCoreDataRepository.GetResources();
                return (IReadOnlyCollection<ResourceDto>) resources.Select(x => resourceDtoFactory.Create(x, x.Age))
                    .ToList();
            },
            version);
    }
}

using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh.Units;
using LazyCache;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class RelicCoreDataService(
    IHohCoreDataRepository hohCoreDataRepository,
    IRelicDtoFactory relicDtoFactory,
    IAppCache appCache,
    ICacheKeyFactory cacheKeyFactory) : IRelicCoreDataService
{
    public Task<IReadOnlyCollection<RelicDto>> GetRelicsAsync()
    {
        return appCache.GetOrAddAsync(cacheKeyFactory.RelicDtos(), CreateRelicsAsync,
            DateTimeOffset.Now.Add(FogConstants.DefaultHohDataEntityCacheTime));
    }

    private async Task<IReadOnlyCollection<RelicDto>> CreateRelicsAsync()
    {
        var relics = await hohCoreDataRepository.GetRelicsAsync();
        var dtos = new List<RelicDto>();
        foreach (var relic in relics)
        {
            dtos.Add(await relicDtoFactory.CreateAsync(relic));
        }

        return dtos;
    }
}

using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class RelicCoreDataService(
    IHohCoreDataRepository hohCoreDataRepository,
    IRelicDtoFactory relicDtoFactory,
    IHohDataCache dataCache,
    ICacheKeyFactory cacheKeyFactory) : IRelicCoreDataService
{
    public Task<IReadOnlyCollection<RelicDto>> GetRelicsAsync()
    {
        var version = hohCoreDataRepository.Version;
        return dataCache.GetOrAddAsync(cacheKeyFactory.RelicDtos(version), CreateRelicsAsync, version);
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

using System.Globalization;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class CommandCenterService(
    IUnitService unitService,
    IHohCoreDataRepository hohCoreDataRepository,
    ICityService cityService,
    IMemoryCache cache,
    IRelicDtoFactory relicDtoFactory,
    ILogger<CommandCenterService> logger) : ICommandCenterService
{
    private const string CACHE_KEY_PREFIX = nameof(CommandCenterService);

    public async Task<CommandCenterDataDto> GetCommandCenterDataAsync()
    {
        if (cache.TryGetValue(GetKey(), out CommandCenterDataDto? cachedResult) && cachedResult != null)
        {
            return cachedResult;
        }

        var heroes = new List<HeroDto>();
        foreach (var hero in await hohCoreDataRepository.GetHeroesAsync())
        {
            var heroDto = await unitService.GetHeroAsync(hero.Id);

            if (heroDto != null)
            {
                heroes.Add(heroDto);
            }
        }

        var barracks = new List<BuildingDto>();
        foreach (var unitType in Enum.GetValues<UnitType>().Where(ut => ut != UnitType.Undefined))
        {
            barracks.AddRange(await cityService.GetBarracks(unitType));
        }

        var result = new CommandCenterDataDto()
        {
            Heroes = heroes,
            Barracks = barracks,
            Relics = relicDtoFactory.Create(await hohCoreDataRepository.GetRelicsAsync(),
                await hohCoreDataRepository.GetRelicBoostAgeModifiersAsync(),
                await hohCoreDataRepository.GetBattleAbilitiesAsync(),
                await hohCoreDataRepository.GetHeroBattleAbilityComponentsAsync()),
        };

        cache.Set(GetKey(), result, TimeSpan.FromHours(1));
        return result;
    }

    private string GetKey()
    {
        return $"{CACHE_KEY_PREFIX}_{CultureInfo.CurrentCulture.Name}";
    }
}

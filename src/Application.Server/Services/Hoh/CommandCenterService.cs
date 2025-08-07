using System.Globalization;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class CommandCenterService(
    IUnitService unitService,
    IHohCoreDataRepository hohCoreDataRepository,
    ICityService cityService,
    IMemoryCache cache) : ICommandCenterService
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

        var result = new CommandCenterDataDto
        {
            Heroes = heroes,
            Barracks = barracks,
        };

        cache.Set(GetKey(), result, TimeSpan.FromHours(1));
        return result;
    }

    private string GetKey()
    {
        return $"{CACHE_KEY_PREFIX}_{CultureInfo.CurrentCulture.Name}";
    }
}

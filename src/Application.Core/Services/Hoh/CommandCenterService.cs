using System.Globalization;
using AutoMapper;
using Ingweland.Fog.Application.Core.Interfaces;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Core.Services.Hoh;

public class CommandCenterService(
    IUnitService unitService,
    IHohCoreDataRepository hohCoreDataRepository,
    ICityService cityService) : ICommandCenterService
{
    public async Task<CommandCenterDataDto> GetCommandCenterDataAsync()
    {
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
        };

        return result;
    }
}

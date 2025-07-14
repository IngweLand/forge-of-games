using AutoMapper;
using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Interfaces;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Core.Services.Hoh;

public class CampaignService(
    IHohCoreDataRepository hohCoreDataRepository,
    IRegionDtoFactory regionDtoFactory,
    IUnitDtoFactory unitDtoFactory,
    IUnitService unitService,
    IMapper mapper,
    IHohGameLocalizationService gameLocalizationService,
    ILogger<CampaignService> logger) : ICampaignService
{
    public async Task<IReadOnlyCollection<ContinentBasicDto>> GetCampaignContinentsBasicDataAsync()
    {
        var world = await hohCoreDataRepository.GetWorldAsync(WorldId.Starter);
        if (world == null)
        {
            logger.LogError($"Failed to get campaign continents by WorldId: {WorldId.Starter}");
            return Array.Empty<ContinentBasicDto>();
        }

        var result = world.Continents.OrderBy(c => c.Index)
            .Select(mapper.Map<ContinentBasicDto>).ToList();
        return result.AsReadOnly();
    }

    public async Task<RegionDto?> GetRegionAsync(RegionId regionId)
    {
        var region = (await hohCoreDataRepository.GetWorldAsync(WorldId.Starter))?.Continents
            .SelectMany(c => c.Regions)
            .FirstOrDefault(r => r.Id == regionId);
        if (region == null)
        {
            logger.LogError($"Failed to get region by RegionId: {regionId}");
            return null;
        }

        var unitIds = region.Encounters.SelectMany(e =>
            e.Details.Values.Where(ed => ed.BattleDetails != null).SelectMany(ed =>
                ed.BattleDetails!.Waves.SelectMany(bw =>
                    bw.Squads.Select(bws => (UnitId: bws.UnitId, IsHero: bws is BattleWaveHeroSquad))))).ToHashSet();
        var units = new List<UnitDto>();
        var heroes = new List<HeroDto>();
        foreach (var t in unitIds)
        {
            var unit = await hohCoreDataRepository.GetUnitAsync(t.UnitId);
            if (unit == null)
            {
                logger.LogError($"Failed to get unit by UnitId: {t.UnitId}");
                return null;
            }

            units.Add(unitDtoFactory.Create(unit, await hohCoreDataRepository.GetUnitStatFormulaData(),
                await hohCoreDataRepository.GetUnitBattleConstants(),
                await hohCoreDataRepository.GetHeroUnitType(unit.Type)));

            if (t.IsHero)
            {
                var hero = await unitService.GetHeroAsync(t.UnitId);
                if (hero != null)
                {
                    heroes.Add(hero);
                }
            }
        }

        return regionDtoFactory.Create(region, units, heroes);
    }

    public Task<RegionBasicDto> GetRegionBasicDataAsync(RegionId regionId)
    {
        return Task.FromResult(new RegionBasicDto
        {
            Id = regionId,
            Name = gameLocalizationService.GetRegionName(regionId),
        });
    }
}

using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class CampaignService(
    IHohCoreDataRepository hohCoreDataRepository,
    IRegionDtoFactory regionDtoFactory,
    IUnitDtoFactory unitDtoFactory,
    IMapper mapper,
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

    public async Task<RegionDto?> GetRegionAsync(RegionId id)
    {
        var region = (await hohCoreDataRepository.GetWorldAsync(WorldId.Starter))?.Continents
            .SelectMany(c => c.Regions)
            .FirstOrDefault(r => r.Id == id);
        if (region == null)
        {
            logger.LogError($"Failed to get region by RegionId: {id}");
            return null;
        }

        var unitIds = region.Encounters.SelectMany(e =>
            e.BattleDetails.Waves.SelectMany(bw => bw.Squads.Select(bws => bws.UnitId))).ToHashSet();
        var units = new List<UnitDto>();
        foreach (var unitId in unitIds)
        {
            var unit = await hohCoreDataRepository.GetUnitAsync(unitId);
            if (unit == null)
            {
                logger.LogError($"Failed to get unit by UnitId: {unitId}");
                return null;
            }

            units.Add(unitDtoFactory.Create(unit, await hohCoreDataRepository.GetUnitStatFormulaData(),
                await hohCoreDataRepository.GetUnitBattleConstants(), await hohCoreDataRepository.GetHeroUnitType(unit.Type)));
        }

        return regionDtoFactory.Create(region, units);
    }
}

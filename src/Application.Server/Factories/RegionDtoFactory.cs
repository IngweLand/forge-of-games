using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Factories;

public class RegionDtoFactory(
    IHohGameLocalizationService localizationService,
    IMapper mapper) : IRegionDtoFactory
{
    public RegionDto Create(Region region, IReadOnlyCollection<UnitDto> units, IReadOnlyCollection<HeroDto> heroes)
    {
        return new RegionDto()
        {
            Id = region.Id,
            Index = region.Index,
            Name = localizationService.GetRegionName(region.Id),
            Encounters = mapper.Map<IReadOnlyCollection<EncounterDto>>(region.Encounters.OrderBy(e => e.Index)),
            Units = units,
            Rewards = region.Rewards.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Rewards),
            Heroes = heroes,
        };
    }
}

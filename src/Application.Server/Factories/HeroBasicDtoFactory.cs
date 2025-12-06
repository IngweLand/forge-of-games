using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.Application.Server.Factories;

public class HeroBasicDtoFactory(IHohGameLocalizationService localizationService) : IHeroBasicDtoFactory
{
    public HeroBasicDto Create(Hero hero, Unit unit, IReadOnlySet<string> abilityTags)
    {
        return new HeroBasicDto
        {
            Id = hero.Id,
            UnitId = unit.Id,
            Name = localizationService.GetUnitName(HohStringParser.GetConcreteId(unit.Id)),
            AssetId = unit.Name,
            UnitType = unit.Type,
            UnitColor = unit.Color,
            ClassId = hero.ClassId,
            StarClass = hero.ProgressionComponent.Id,
            AbilityTags = abilityTags,
        };
    }
}

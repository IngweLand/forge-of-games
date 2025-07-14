using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.Application.Core.Factories;

public class HeroBasicDtoFactory(IHohGameLocalizationService localizationService) : IHeroBasicDtoFactory
{
    public HeroBasicDto Create(Hero hero, Unit unit)
    {
        return new HeroBasicDto
        {
            Id = hero.Id,
            UnitId = unit.Id,
            Name = localizationService.GetUnitName(HohStringParser.GetConcreteId(unit.Id)),
            AssetId = unit.Name,
            UnitType = unit.Type,
            UnitColor = unit.Color,
        };
    }
}

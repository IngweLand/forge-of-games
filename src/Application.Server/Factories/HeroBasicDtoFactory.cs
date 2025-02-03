using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Helpers;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Server.Factories;

public class HeroBasicDtoFactory(IHohGameLocalizationService localizationService) : IHeroBasicDtoFactory
{
    public HeroBasicDto Create(Hero hero, Unit unit)
    {
        return new HeroBasicDto
        {
            Id = hero.Id,
            Name = localizationService.GetUnitName(StringParser.GetConcreteId(unit.Id)),
            AssetId = unit.Name,
            UnitType = unit.Type,
            UnitColor = unit.Color,
        };
    }
}

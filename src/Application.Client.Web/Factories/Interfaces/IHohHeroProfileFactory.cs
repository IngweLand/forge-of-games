using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IHohHeroProfileFactory
{
    HeroProfile Create(HeroProfileIdentifier profileIdentifier, HeroDto hero, BuildingDto? barracks);
    HeroProfile Create(IBattleUnitProperties battleUnit, HeroDto hero, BuildingDto? barracks);
}

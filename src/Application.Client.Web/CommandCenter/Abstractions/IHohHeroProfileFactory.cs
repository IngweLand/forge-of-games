using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface IHohHeroProfileFactory
{
    HeroProfile Create(BasicHeroProfile profileDto, HeroDto hero, BuildingDto? barracks);
    HeroProfile Create(HeroProfile profile, HeroDto hero, BuildingDto? barracks);
    HeroProfile Create(BattleUnitDto battleUnit, HeroDto hero, BuildingDto? barracks);
}

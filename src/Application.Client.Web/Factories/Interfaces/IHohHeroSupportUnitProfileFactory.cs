using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IHohHeroSupportUnitProfileFactory
{
    HeroSupportUnitProfile Create(UnitDto baseSupportUnit, BuildingDto? barracks);
}

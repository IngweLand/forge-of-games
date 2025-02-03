using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IHeroSupportUnitViewModelFactory
{
    HeroSupportUnitViewModel? Create(UnitDto baseSupportUnit, BuildingDto? barracks);
    HeroSupportUnitViewModel Create(HeroSupportUnitProfile profile);
}

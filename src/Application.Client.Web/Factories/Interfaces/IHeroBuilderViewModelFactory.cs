using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IHeroBuilderViewModelFactory
{
    HeroBuilderViewModel Create(HeroDto hero, BattleAbilityDto ability, IReadOnlyCollection<BuildingDto> barracks);
}

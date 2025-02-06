using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IHeroAbilityViewModelFactory
{
    HeroAbilityViewModel Create(BattleAbilityDto battleAbilityDto);
}

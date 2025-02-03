using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IHeroAbilityDtoFactory
{
    HeroAbilityDto Create(HeroBattleAbilityComponent abilityComponent, IList<HeroAbility> abilities);
}

using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IBattleAbilityDtoFactory
{
    BattleAbilityDto CreateForHero(HeroBattleAbilityComponent abilityComponent, IList<BattleAbility> abilities);
}

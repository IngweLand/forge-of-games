using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IRelicDtoFactory
{
    IReadOnlyCollection<RelicDto> Create(IEnumerable<Relic> relics,
        IEnumerable<RelicBoostAgeModifier> ageModifiers, IEnumerable<BattleAbility> abilities,
        IReadOnlyCollection<HeroBattleAbilityComponent> abilityComponents);
}

using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IRelicBattleAbilityDtoFactory
{
    RelicBattleAbilityDto Create(BattleAbility ability);
}

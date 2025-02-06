using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Server.Factories;

public class RelicBattleAbilityDtoFactory(IHohGameLocalizationService hohGameLocalizationService)
    : IRelicBattleAbilityDtoFactory
{
    public RelicBattleAbilityDto Create(BattleAbility ability)
    {
        var description = hohGameLocalizationService.GetHeroAbilityDescription(ability.DescriptionLocalizationId);

        return new RelicBattleAbilityDto
        {
            Id = ability.Id,
            Description = description,
            DescriptionItems = ability.DescriptionItems,
        };
    }
}

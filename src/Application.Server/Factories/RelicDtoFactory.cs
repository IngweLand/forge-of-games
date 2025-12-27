using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Relics;
using Ingweland.Fog.Shared.Helpers;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Factories;

public class RelicDtoFactory(
    IHohCoreDataRepository hohCoreDataRepository,
    IHohGameLocalizationService hohGameLocalizationService,
    ILogger<RelicDtoFactory> logger)
    : IRelicDtoFactory
{
    public async Task<RelicDto> CreateAsync(Relic relic)
    {
        var levels = new List<RelicLevelDto>();
        foreach (var level in relic.LevelData)
        {
            var dto = await CreateLevel(level);
            if (dto == null)
            {
                continue;
            }

            levels.Add(dto);
        }

        return new RelicDto
        {
            Id = relic.Id,
            LevelData = levels,
            Rarity = relic.Rarity,
            Name = hohGameLocalizationService.GetRelicName(HohStringParser.GetConcreteId(relic.Id)),
        };
    }

    private async Task<RelicLevelDto?> CreateLevel(RelicLevel level)
    {
        var abilities = new List<BattleAbilityDto>();
        foreach (var abilityId in level.Abilities)
        {
            var ability = await hohCoreDataRepository.GetHeroAbilityAsync(abilityId);
            if (ability == null)
            {
                logger.LogWarning("Could not find ability for relic with id {Id}", abilityId);
                return null;
            }

            abilities.Add(new BattleAbilityDto
            {
                Id = abilityId,
                DescriptionItems = ability.DescriptionItems,
                Description = ability.DescriptionLocalizationId != null
                    ? hohGameLocalizationService.GetBattleAbilityDescription(ability.DescriptionLocalizationId)
                    : null,
            });
        }

        return new RelicLevelDto
        {
            Level = level.Level,
            Ascension = level.Ascension,
            AscensionLevel = level.AscensionLevel,
            Abilities = abilities,
        };
    }
}

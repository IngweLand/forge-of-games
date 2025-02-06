using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Factories;

public class RelicDtoFactory(
    IHohGameLocalizationService hohGameLocalizationService,
    IRelicBattleAbilityDtoFactory relicBattleAbilityDtoFactory) : IRelicDtoFactory
{
    public IReadOnlyCollection<RelicDto> Create(IEnumerable<Relic> relics,
        IEnumerable<RelicBoostAgeModifier> ageModifiers, IEnumerable<BattleAbility> abilities,
        IReadOnlyCollection<HeroBattleAbilityComponent> abilityComponents)
    {
        var ageModifierDic = ageModifiers.ToDictionary(src => src.Id);
        var abilityDic = abilities.ToDictionary(src => src.Id);
        var relicDtos = new List<RelicDto>();
        foreach (var relic in relics)
        {
            // Original data has unreleased features. Filter them out.
            var nameLocalizationId = abilityComponents.FirstOrDefault(src => src.HeroAbilityId.EndsWith(relic.Id))
                ?.HeroAbilityId;
            if (nameLocalizationId == null)
            {
                continue;
            }

            var levels = new List<RelicLevelDto>();
            foreach (var relicLevel in relic.Levels)
            {
                var boosts = relicLevel.Boosts.Select(relicLevelBoost => new RelicBoostDto()
                {
                    StatBoost = relicLevelBoost.StatBoost,
                    AgeModifiers = ageModifierDic[relicLevelBoost.RelicBoostAgeModifierId].AgeModifiers,
                }).ToList();

                levels.Add(new RelicLevelDto()
                {
                    Level = relicLevel.Level,
                    AscensionLevel = relicLevel.AscensionLevel,
                    IsAscension = relicLevel.IsAscension,
                    Boosts = boosts,
                    Ability = relicBattleAbilityDtoFactory.Create(abilityDic[relicLevel.Abilities.First()]),
                });
            }

            relicDtos.Add(new RelicDto()
            {
                Id = relic.Id,
                Name = hohGameLocalizationService.GetHeroAbilityName(nameLocalizationId),
                RarityId = relic.RarityId,
                HeroEquipFilter = relic.HeroEquipFilter?.HeroClassId ?? HeroClassId.Undefined,
                Levels = levels,
            });
        }

        return relicDtos;
    }
}

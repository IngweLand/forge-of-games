using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Server.Factories;

public class HeroAbilityDtoFactory(IHohGameLocalizationService hohGameLocalizationService) : IHeroAbilityDtoFactory
{
    public HeroAbilityDto Create(HeroBattleAbilityComponent abilityComponent, IList<HeroAbility> abilities)
    {
        var levels = new List<HeroAbilityLevelDto>();
        foreach (var levelData in abilityComponent.Levels.OrderBy(c => int.Parse(c.AbilityId.Split('_').Last())))
        {
            var level = int.Parse(levelData.AbilityId.Split('_').Last());
            var ability = abilities.First(ha => ha.Id == levelData.AbilityId);
            var description = levelData.IsKeyLevel || level == 1
                ? hohGameLocalizationService.GetHeroAbilityDescription(ability.DescriptionLocalizationId)
                : null;

            levels.Add(new HeroAbilityLevelDto
            {
                Level = level,
                Description = description,
                Cost = levelData.Cost,
                DescriptionItems = ability.DescriptionItems,
            });
        }

        return new HeroAbilityDto
        {
            Id = abilityComponent.HeroAbilityId,
            Levels = levels,
            Name = hohGameLocalizationService.GetHeroAbilityName(abilityComponent.HeroAbilityId),
        };
    }
}

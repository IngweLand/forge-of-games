using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class HeroBuilderViewModelFactory(IHohHeroLevelSpecsProvider heroLevelSpecsProvider)
    : IHeroBuilderViewModelFactory
{
    public HeroBuilderViewModel Create(HeroDto hero, BattleAbilityDto ability, IReadOnlyCollection<BuildingDto> barracks)
    {
        return new HeroBuilderViewModel()
        {
            HeroLevels = heroLevelSpecsProvider.Get(hero.ProgressionCosts.Count),
            AbilityLevels = Enumerable.Range(1, ability.Levels.Count).ToList(),
            AwakeningLevels = Enumerable.Range(0, 6).ToList(),
            BarracksLevels = Enumerable.Range(0, barracks.Count + 1).ToList(),
            Hero = hero,
            Ability = ability,
            Barracks = barracks,
        };
    }
}

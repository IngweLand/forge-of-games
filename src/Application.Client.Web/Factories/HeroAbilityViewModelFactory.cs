using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class HeroAbilityViewModelFactory(IAssetUrlProvider assetUrlProvider) : IHeroAbilityViewModelFactory
{
    public HeroAbilityViewModel Create(HeroAbilityDto heroAbilityDto)
    {
        var levels = new List<HeroAbilityLevelViewModel>();
        HeroAbilityText abilityText = null!;
        foreach (var levelDto in heroAbilityDto.Levels.OrderBy(l => l.Level))
        {
            if (levelDto.Description != null)
            {
                abilityText = new HeroAbilityText(levelDto.Description);
            }

            var level = new HeroAbilityLevelViewModel
            {
                Title = abilityText.Title,
                Description = abilityText.GetDescription(levelDto.DescriptionItems),
                Cost = levelDto.Cost,
                Level = levelDto.Level,
            };
            levels.Add(level);
        }

        return new HeroAbilityViewModel
        {
            IconUrl = assetUrlProvider.GetHohHeroAbilityIconUrl(heroAbilityDto.Id),
            Name = heroAbilityDto.Name,
            Levels = levels,
        };
    }
}

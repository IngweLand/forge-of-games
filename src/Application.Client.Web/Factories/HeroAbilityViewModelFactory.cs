using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class HeroAbilityViewModelFactory(IAssetUrlProvider assetUrlProvider) : IHeroAbilityViewModelFactory
{
    public HeroAbilityViewModel Create(HeroAbilityDto heroAbilityDto, int level, float abilityChargeTime,
        float abilityInitialChargeTime)
    {
        var abilityLevels = heroAbilityDto.Levels.Take(level).ToList();
        var abilityText = new HeroAbilityText(abilityLevels.Last(hal => hal.Description != null).Description!);
        var lastLevel = abilityLevels.Last();
        return new HeroAbilityViewModel
        {
            IconUrl = assetUrlProvider.GetHohHeroAbilityIconUrl(heroAbilityDto.Id),
            Name = heroAbilityDto.Name,
            Level = new HeroAbilityLevelViewModel
            {
                Title = abilityText.Title,
                Description = abilityText.GetDescription(lastLevel.DescriptionItems),
                Cost = lastLevel.Cost,
                Level = lastLevel.Level,
            },
            ChargeTime = $"{abilityChargeTime:F1}s",
            InitialChargeTime = $"{abilityInitialChargeTime:F1}s",
            InitialChargePercentage = MathF.Round(
                (abilityChargeTime - abilityInitialChargeTime) / abilityChargeTime * 100),
        };
    }
}

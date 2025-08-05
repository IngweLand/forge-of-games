using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class HeroRelicViewModelFactory(IAssetUrlProvider assetUrlProvider) : IHeroRelicViewModelFactory
{
    public HeroRelicViewModel Create(RelicDto relic, RelicLevelDto level)
    {
        var ability = level.Abilities.FirstOrDefault(x => x.Description != null);
        HeroAbilityText? abilityText = null;
        if (ability != null)
        {
            abilityText = new HeroAbilityText(ability.Description!);
        }
        
        return new HeroRelicViewModel()
        {
            Id = relic.Id,
            Name = relic.Name,
            Description = abilityText?.GetDescription(ability!.DescriptionItems) ?? string.Empty,
            StarCount = relic.Rarity.ToStarCount(),
            Level = Math.Min(10, level.Level).ToString(),
            Ascension = level.Ascension,
            AscensionLevel = level.AscensionLevel,
            IconUrl = assetUrlProvider.GetHohRelicIconUrl(HohStringParser.GetConcreteId(relic.Id)),
        };
    }
}

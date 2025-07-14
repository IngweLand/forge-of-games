using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

namespace Ingweland.Fog.Application.Core.Mapping.Converters;

public class HeroAbilityDescriptionLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<string, string>
{
    public string Convert(string abilityDescriptionId, ResolutionContext context)
    {
        return localizationService.GetHeroAbilityDescription(abilityDescriptionId);
    }
}

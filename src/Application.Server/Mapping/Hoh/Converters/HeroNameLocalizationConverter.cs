using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;

public class HeroNameLocalizationConverter : IValueConverter<string, string>
{
    private readonly IHohGameLocalizationService _localizationService;

    public HeroNameLocalizationConverter(IHohGameLocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public string Convert(string heroId, ResolutionContext context) => _localizationService.GetHeroName(heroId);
}

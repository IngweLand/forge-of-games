using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Mapping.Converters;

public class ContinentNameLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<ContinentId, string>
{
    public string Convert(ContinentId continentId, ResolutionContext context)
    {
        return localizationService.GetContinentName(continentId);
    }
}

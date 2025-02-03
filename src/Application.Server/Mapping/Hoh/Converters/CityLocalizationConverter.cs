using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;

public class CityLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<CityId, string>
{
    public string Convert(CityId cityId, ResolutionContext context)
    {
        return localizationService.GetCityName(cityId);
    }
}

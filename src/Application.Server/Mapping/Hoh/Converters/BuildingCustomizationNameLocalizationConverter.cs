using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;

public class BuildingCustomizationNameLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<string, string>
{
    public string Convert(string customizationId, ResolutionContext context)
    {
        return localizationService.GetBuildingCustomizationName(customizationId);
    }
}

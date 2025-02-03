using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;

public class BuildingTypeLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<BuildingType, string>
{
    public string Convert(BuildingType buildingType, ResolutionContext context)
    {
        return localizationService.GetBuildingType(buildingType);
    }
}

using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Mapping.Converters;

public class BuildingGroupLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<BuildingGroup, string>
{
    public string Convert(BuildingGroup buildingGroup, ResolutionContext context)
    {
        return localizationService.GetBuildingGroup(buildingGroup);
    }
}

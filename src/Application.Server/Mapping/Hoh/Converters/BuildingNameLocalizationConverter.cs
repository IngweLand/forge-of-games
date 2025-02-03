using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;

public class BuildingNameLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<string, string>
{
    public string Convert(string buildingId, ResolutionContext context)
    {
        return localizationService.GetBuildingName(buildingId);
    }
}

using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;

public class RegionNameLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<RegionId, string>
{
    public string Convert(RegionId regionId, ResolutionContext context)
    {
        return localizationService.GetRegionName(regionId);
    }
}

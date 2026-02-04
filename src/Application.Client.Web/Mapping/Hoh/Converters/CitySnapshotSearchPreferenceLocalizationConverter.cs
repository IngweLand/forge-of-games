using AutoMapper;
using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Models.Fog.Enums;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class CitySnapshotSearchPreferenceLocalizationConverter(IStringLocalizer<FogResource> localizer)
    : IValueConverter<CitySnapshotSearchPreference, string>
{
    public string Convert(CitySnapshotSearchPreference sourceMember, ResolutionContext context)
    {
        return sourceMember switch
        {
            CitySnapshotSearchPreference.Goods => localizer[FogResource.CityPlanner_Inspirations_Goods],
            CitySnapshotSearchPreference.Coins => localizer[FogResource.CityPlanner_Inspirations_Coins],
            CitySnapshotSearchPreference.Food => localizer[FogResource.CityPlanner_Inspirations_Food],
            _ => sourceMember.ToString(),
        };
    }
}

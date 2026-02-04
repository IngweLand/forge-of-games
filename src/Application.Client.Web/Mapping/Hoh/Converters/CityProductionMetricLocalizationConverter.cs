using AutoMapper;
using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Models.Fog.Enums;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class CityProductionMetricLocalizationConverter(IStringLocalizer<FogResource> localizer)
    : IValueConverter<CityProductionMetric, string>
{
    public string Convert(CityProductionMetric sourceMember, ResolutionContext context)
    {
        return sourceMember switch
        {
            CityProductionMetric.Storage => localizer[FogResource.CityProductionMetric_Storage],
            CityProductionMetric.OneHour => localizer[FogResource.CityProductionMetric_OneHour],
            CityProductionMetric.OneDay => localizer[FogResource.CityProductionMetric_OneDay],
            CityProductionMetric.StoragePerCityArea => localizer[FogResource.CityProductionMetric_StoragePerCityArea],
            CityProductionMetric.OneHourPerCityArea => localizer[FogResource.CityProductionMetric_OneHourPerCityArea],
            CityProductionMetric.OneDayPerCityArea => localizer[FogResource.CityProductionMetric_OneDayPerCityArea],
            _ => sourceMember.ToString(),
        };
    }
}

using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;
using Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;
using Ingweland.Fog.Application.Client.Web.ViewModels;
using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class CityInspirationsUiProfile : Profile
{
    public CityInspirationsUiProfile()
    {
        CreateMap<CitySnapshotSearchPreference, CitySnapshotSearchPreferenceViewModel>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Name,
                opt =>
                    opt.ConvertUsing<CitySnapshotSearchPreferenceLocalizationConverter,
                        CitySnapshotSearchPreference>(src => src));

        CreateMap<CityProductionMetric, LabeledValue<CityProductionMetric>>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Label,
                opt => opt.ConvertUsing<CityProductionMetricLocalizationConverter, CityProductionMetric>(src => src));
    }
}

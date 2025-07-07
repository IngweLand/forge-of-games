using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;
using Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;
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
    }
}

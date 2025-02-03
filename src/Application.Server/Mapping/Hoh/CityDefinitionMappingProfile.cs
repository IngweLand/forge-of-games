using AutoMapper;
using Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class CityDefinitionMappingProfile : Profile
{
    public CityDefinitionMappingProfile()
    {
        CreateMap<Building, BuildingGroupBasicDto>()
            .ForMember(dest => dest.Name,
                opt => opt.ConvertUsing<BuildingGroupLocalizationConverter, BuildingGroup>(src => src.Group))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Group));

        CreateMap<Building, BuildingDto>()
            .ForMember(dest => dest.Name,
                opt => opt.ConvertUsing<BuildingNameLocalizationConverter, string>(src => src.Id))
            .ForMember(dest => dest.GroupName,
                opt => opt.ConvertUsing<BuildingGroupLocalizationConverter, BuildingGroup>(src => src.Group))
            .ForMember(dest => dest.TypeName,
                opt => opt.ConvertUsing<BuildingTypeLocalizationConverter, BuildingType>(src => src.Type));

        CreateMap<Wonder, WonderBasicDto>()
            .ForMember(dest => dest.CityName,
                opt => opt.ConvertUsing<CityLocalizationConverter, CityId>(src => src.CityId))
            .ForMember(dest => dest.WonderName,
                opt => opt.ConvertUsing<WonderNameLocalizationConverter, string>(src => $"{src.CityId}_{src.Id}"));

        CreateMap<BuildingCustomization, BuildingCustomizationDto>()
            .ForMember(dest => dest.Name,
                opt => opt.ConvertUsing<BuildingCustomizationNameLocalizationConverter, string>(src => src.Id));
    }
}

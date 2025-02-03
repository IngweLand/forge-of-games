using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class CityUiProfile : Profile
{
    public CityUiProfile()
    {
        CreateMap<BuildingTypeDto, BuildingTypeViewModel>();
        CreateMap<BuildingGroupBasicDto, BuildingGroupBasicViewModel>();
        CreateMap<BuildingGroupDto, BuildingGroupViewModel>().ConvertUsing<BuildingGroupDtoToViewModelConverter>();

        CreateMap<ConstructionComponent, ConstructionComponentViewModel>()
            .ForMember(dest => dest.BuildTime, opt => opt.ConvertUsing<BuildingTimeConverter, int>());
        CreateMap<UpgradeComponent, UpgradeComponentViewModel>()
            .ForMember(dest => dest.UpgradeTime, opt => opt.ConvertUsing<BuildingTimeConverter, int>());

        CreateMap<WonderBasicDto, WonderBasicViewModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.WonderName))
            .ForMember(dest => dest.ImageUrl,
                opt => opt.ConvertUsing<WonderBasicToWonderImageUrlConverter, WonderBasicDto>(src => src));
    }
}

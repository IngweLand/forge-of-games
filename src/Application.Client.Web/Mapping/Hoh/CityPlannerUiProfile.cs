using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class CityPlannerUiProfile :Profile
{
    public CityPlannerUiProfile()
    {
        CreateMap<BuildingDto, BuildingSelectorItemViewModel>()
            .ForMember(dest => dest.BuildingGroup, opt=> opt.MapFrom(src => src.Group))
            .ForMember(dest => dest.Label, opt=> opt.MapFrom(src => src.GroupName));

        CreateMap<CityMapEntity, HohCityMapEntity>()
            .ForMember(dest => dest.X, opt => opt.MapFrom(src => src.Location.X))
            .ForMember(dest => dest.Y, opt => opt.MapFrom(src => src.Location.Y));
    }
}

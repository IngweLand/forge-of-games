using System.Drawing;
using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using CityMapEntity = Ingweland.Fog.Application.Client.Web.CityPlanner.CityMapEntity;

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

        CreateMap<CityCultureAreaComponent, MapAreaHappinessProvider>()
            .ForMember(dest => dest.Bounds,
                opt => opt.MapFrom(src => new Rectangle(src.X, src.Y, src.Width, src.Height)));
    }
}

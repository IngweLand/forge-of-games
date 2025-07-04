using System.Drawing;
using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using CityMapEntity = Ingweland.Fog.Application.Core.CityPlanner.CityMapEntity;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class CityPlannerUiProfile :Profile
{
    public CityPlannerUiProfile()
    {
        CreateMap<BuildingDto, BuildingSelectorItemViewModel>()
            .ForMember(dest => dest.BuildingGroup, opt=> opt.MapFrom(src => src.Group))
            .ForMember(dest => dest.Label, opt=> opt.MapFrom(src => src.GroupName));
    }
}

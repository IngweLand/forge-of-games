using System.Drawing;
using AutoMapper;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using CityMapEntity = Ingweland.Fog.Application.Core.CityPlanner.CityMapEntity;

namespace Ingweland.Fog.Application.Core.Mapping;

public class CityPlannerProfile : Profile
{
    public CityPlannerProfile()
    {
        CreateMap<CityMapEntity, HohCityMapEntity>()
            .ForMember(dest => dest.X, opt => opt.MapFrom(src => src.Location.X))
            .ForMember(dest => dest.Y, opt => opt.MapFrom(src => src.Location.Y))
            .ForMember(dest => dest.SelectedProductId, opt =>
            {
                opt.PreCondition(src => src.SelectedProduct != null);
                opt.MapFrom(src => src.SelectedProduct!.Id);
            });

        CreateMap<CityCultureAreaComponent, MapAreaHappinessProvider>()
            .ForMember(dest => dest.Bounds,
                opt => opt.MapFrom(src => new Rectangle(src.X, src.Y, src.Width, src.Height)));

        CreateMap<HohCity, HohCityBasicData>();
        CreateMap<CityStrategy, HohCityBasicData>();
    }
}

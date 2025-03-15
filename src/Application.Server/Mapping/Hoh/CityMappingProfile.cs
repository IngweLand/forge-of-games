using AutoMapper;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class CityMappingProfile : Profile
{
    public CityMappingProfile()
    {
        CreateMap<CityMapEntity, HohCityMapEntity>()
            .ForMember(dest => dest.SelectedProductId, opt => opt.MapFrom(src =>
                src.Productions
                    .Where(p => p.Source == ProductionSourceConstant.Main && p.IsStarted)
                    .Select(p => p.DefinitionId)
                    .FirstOrDefault())
            );
    }
}
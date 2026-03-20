using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class UnitMappingProfile : Profile
{
    public UnitMappingProfile()
    {
        CreateMap<HeroAbilityFeaturesEntity, HeroAbilityFeaturesDto>();
    }
}

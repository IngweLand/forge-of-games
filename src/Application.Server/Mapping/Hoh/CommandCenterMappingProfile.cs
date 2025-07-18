using AutoMapper;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Helpers;
using HeroProfile = Ingweland.Fog.Inn.Models.Hoh.HeroProfile;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class CommandCenterMappingProfile : Profile
{
    public CommandCenterMappingProfile()
    {
        CreateMap<HeroProfile, HeroProfileIdentifier>()
            .ForMember(dest => dest.HeroId, opt => opt.MapFrom(src => HohStringParser.GetConcreteId(src.HeroId)));
    }
}

using AutoMapper;
using Ingweland.Fog.Application.Server.Helpers;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using HeroProfile = Ingweland.Fog.Inn.Models.Hoh.HeroProfile;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class CommandCenterMappingProfile:Profile
{
    public CommandCenterMappingProfile()
    {
        CreateMap<HeroProfile, BasicHeroProfile>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString("N")))
            .ForMember(dest => dest.HeroId, opt => opt.MapFrom(src => StringParser.GetConcreteId(src.HeroId)));
    }
}

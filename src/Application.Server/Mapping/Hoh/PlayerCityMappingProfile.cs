using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class PlayerCityMappingProfile : Profile
{
    public PlayerCityMappingProfile()
    {
        CreateMap<PlayerCitySnapshot, PlayerCitySnapshotBasicDto>()
            .ForMember(dest => dest.PlayerName, opt => opt.MapFrom(x => x.Player.Name));
    }
}

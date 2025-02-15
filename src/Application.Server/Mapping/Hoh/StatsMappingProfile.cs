using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class StatsMappingProfile:Profile
{
    public StatsMappingProfile()
    {
        CreateMap<Player, PlayerKey>();
        CreateMap<PlayerRanking, PlayerRanking>();
        CreateMap<Player, Player>()
            .ForMember(dest => dest.Rankings, opt => opt.Ignore());
        CreateMap<Player, PlayerDto>();
    }
}

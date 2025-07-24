using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.Battle;
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
        CreateMap<PlayerRanking, PlayerKey>();
        
        CreateMap<Alliance, AllianceKey>();
        CreateMap<AllianceRanking, AllianceRanking>();
        CreateMap<Alliance, Alliance>()
            .ForMember(dest => dest.Rankings, opt => opt.Ignore());
        CreateMap<Alliance, AllianceDto>();
        
        CreateMap<PvpBattle, BattleKey>();

        CreateMap<ProfileSquadEntity, ProfileSquadDto>();
    }
}

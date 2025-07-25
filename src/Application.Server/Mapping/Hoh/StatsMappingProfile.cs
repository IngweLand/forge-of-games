using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Constants;
using Ingweland.Fog.Shared.Extensions;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class StatsMappingProfile : Profile
{
    public StatsMappingProfile()
    {
        CreateMap<Player, PlayerKey>();
        CreateMap<PlayerRanking, PlayerRanking>();
        CreateMap<Player, Player>()
            .ForMember(dest => dest.Rankings, opt => opt.Ignore());
        CreateMap<Player, PlayerDto>()
            .ForMember(dest => dest.Alliance, opt =>
            {
                opt.PreCondition(x => x.CurrentAlliance != null);
                opt.MapFrom(x => x.CurrentAlliance);
            });
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

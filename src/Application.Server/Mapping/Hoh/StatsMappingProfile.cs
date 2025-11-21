using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
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
            .ForMember(dest => dest.AllianceId, opt =>
            {
                opt.PreCondition(x => x.AllianceMembership != null);
                opt.MapFrom(x => x.AllianceMembership!.AllianceId);
            })
            .ForMember(dest => dest.AllianceName, opt =>
            {
                opt.PreCondition(x => x.AllianceMembership != null);
                opt.MapFrom(x => x.AllianceMembership!.Alliance.Name);
            });
        CreateMap<PlayerRanking, PlayerKey>();

        CreateMap<Alliance, AllianceKey>();
        CreateMap<AllianceRanking, AllianceRanking>();
        CreateMap<Alliance, Alliance>()
            .ForMember(dest => dest.Rankings, opt => opt.Ignore());
        CreateMap<Alliance, AllianceDto>()
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.Status == InGameEntityStatus.Missing))
            .ForMember(dest => dest.UpdatedAt,
                opt => opt.MapFrom(src =>
                    src.UpdatedAt > src.MembersUpdatedAt.ToDateOnly()
                        ? src.UpdatedAt
                        : src.MembersUpdatedAt.ToDateOnly()));

        CreateMap<PvpBattle, BattleKey>();

        CreateMap<ProfileSquadEntity, ProfileSquadDto>()
            .ForMember(dest => dest.Hero, opt => opt.MapFrom(x => x.Data.Hero))
            .ForMember(dest => dest.SupportUnit, opt => opt.MapFrom(x => x.Data.SupportUnit));

        CreateMap<EquipmentInsightsEntity, EquipmentInsightsDto>();
        CreateMap<RelicInsightsEntity, RelicInsightsDto>();
        CreateMap<PvpRanking2, PvpRankingDto>();
    }
}

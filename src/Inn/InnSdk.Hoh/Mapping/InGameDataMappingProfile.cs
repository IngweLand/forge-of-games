using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.InnSdk.Hoh.Mapping;

public class InGameDataMappingProfile : Profile
{
    public InGameDataMappingProfile()
    {
        CreateMap<PlayerRankDto, PlayerRank>()
            .ForMember(dest => dest.AllianceName, opt => opt.PreCondition(src => src.HasAllianceName));
        CreateMap<PlayerRanksDTO, PlayerRanks>();
        CreateMap<AllianceRankDto, AllianceRank>()
            .ForMember(dest => dest.RegisteredAt, opt => opt.MapFrom(src => src.Info.RegisteredAt.ToDateTime()))
            .ForMember(dest => dest.Leader, opt => opt.MapFrom(src => src.Info.Leader));
        CreateMap<AllianceRanksDTO, AllianceRanks>();

        CreateMap<PvpRankDto, PvpRank>();
        CreateMap<PlayerDto, HohPlayer>();
        CreateMap<AllianceDto, HohAlliance>();
        CreateMap<AllianceMembersResponse, AllianceWithMembers>();
        CreateMap<AllianceMemberDto, AllianceMember>();
        CreateMap<HeroTreasureHuntAlliancePointsPush, HeroTreasureHuntAlliancePoints>();
        CreateMap<HeroTreasureHuntPlayerPointsPush, HeroTreasureHuntPlayerPoints>();
        CreateMap<AlliancePush, HohAlliance>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Alliance.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Alliance.Details.Name))
            .ForMember(dest => dest.AvatarIconId, opt => opt.MapFrom(src => src.Alliance.Details.AvatarIconId))
            .ForMember(dest => dest.AvatarBackgroundId,
                opt => opt.MapFrom(src => src.Alliance.Details.AvatarBackgroundId))
            .ForMember(dest => dest.MemberCount, opt => opt.MapFrom(src => src.Alliance.MemberCount));

        CreateMap<CommunicationDto, Wakeup>()
            .ForMember(dest => dest.Alliance, opt => opt.MapFrom(src => src.AlliancePush))
            .ForMember(dest => dest.AllianceWithMembers, opt => opt.MapFrom(src => src.AllianceMembersResponse))
            .ForMember(dest => dest.AthAllianceRankings,
                opt => opt.MapFrom(src => src.HeroTreasureHuntAlliancePointsPushs))
            .ForMember(dest => dest.AthPlayerRankings,
                opt => opt.MapFrom(src => src.HeroTreasureHuntPlayerPointsPushs));

        CreateMap<PvpUnitDto, PvpUnit>();
        CreateMap<PvpUnitDetailsDto, PvpUnitDetails>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BaseProps.Id))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.BaseProps.Level))
            .ForMember(dest => dest.AscensionLevel, opt => opt.MapFrom(src => src.BaseProps.AscensionLevel))
            .ForMember(dest => dest.AbilityLevel, opt => opt.MapFrom(src => src.BaseProps.AbilityLevel))
            .ForMember(dest => dest.Abilities, opt => opt.MapFrom(src => src.BaseProps.Abilities));
        CreateMap<PvpBattleDto, PvpBattle>()
            .ForMember(dest => dest.PerformedAt, opt => opt.MapFrom(src => src.PerformedAt.ToDateTime()));
    }
}
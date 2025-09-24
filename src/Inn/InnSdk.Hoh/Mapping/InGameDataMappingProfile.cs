using AutoMapper;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Mapping.Converters;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Entities.Research;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;
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
        CreateMap<PlayerDto, HohPlayer>()
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => HohStringParser.GetConcreteId(src.Age)));
        CreateMap<AllianceDto, HohAlliance>();
        CreateMap<AllianceMembersResponse, AllianceWithMembers>();
        CreateMap<AllianceMemberDto, AllianceMember>()
            .ForMember(dest => dest.Role,
                opt => opt.MapFrom(src =>
                    HohStringParser.ParseEnumFromString<AllianceMemberRole>(src.RoleDetails.Role)))
            .ForMember(dest => dest.JoinedAt, opt => opt.MapFrom(src => src.JoinedAt.ToDateTime()))
            .ForMember(dest => dest.LastSeenOnline,
                opt => opt.MapFrom(src => DateTime.UtcNow.AddSeconds(-src.SecondsSinceLastOnline)));
        CreateMap<AllianceSearchResultDto, AllianceSearchResult>()
            .ForMember(dest => dest.Alliance, opt => opt.MapFrom(src => src.Details));
        CreateMap<AllianceSearchResultDetailsDto, HohAllianceExtended>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Facade.Name))
            .ForMember(dest => dest.AvatarIconId, opt => opt.MapFrom(src => src.Facade.AvatarIconId))
            .ForMember(dest => dest.AvatarBackgroundId, opt => opt.MapFrom(src => src.Facade.AvatarBackgroundId))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Facade.Description));
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
            .ForMember(dest => dest.Alliance, opt =>
            {
                opt.PreCondition(src => src.AlliancePush != null);
                opt.MapFrom(src => src.AlliancePush);
            })
            .ForMember(dest => dest.AllianceWithMembers, opt => opt.MapFrom(src => src.AllianceMembersResponse))
            .ForMember(dest => dest.AthAllianceRankings,
                opt => opt.MapFrom(src => src.HeroTreasureHuntAlliancePointsPushs))
            .ForMember(dest => dest.AthPlayerRankings,
                opt => opt.MapFrom(src => src.HeroTreasureHuntPlayerPointsPushs))
            .ForMember(dest => dest.Leaderboards, opt => opt.MapFrom(src => src.Leaderboards));

        CreateMap<PvpBattleDto, PvpBattle>()
            .ForMember(dest => dest.PerformedAt, opt => opt.MapFrom(src => src.PerformedAt.ToDateTime()));

        CreateMap<EquipmentItemDto, EquipmentItem>()
            .ForMember(dest => dest.EquippedOnHero, opt =>
            {
                opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.EquippedOnHeroDefinitionId));
                opt.MapFrom(src => HohStringParser.GetConcreteId(src.EquippedOnHeroDefinitionId));
            })
            .ForMember(dest => dest.EquipmentSlotType,
                opt => opt.MapFrom(src =>
                    HohStringParser.ParseEnumFromString<EquipmentSlotType>(src.EquipmentSlotTypeDefinitionId)))
            .ForMember(dest => dest.EquipmentRarity,
                opt => opt.ConvertUsing<EquipmentRarityValueConverter, string>(src => src.EquipmentRarityDefinitionId))
            .ForMember(dest => dest.EquipmentSet,
                opt => opt.MapFrom(src =>
                    HohStringParser.ParseEnumFromString<EquipmentSet>(src.EquipmentSetDefinitionId)));
        CreateMap<SquadEquipmentItemDto, SquadEquipmentItem>()
            .ForMember(dest => dest.EquipmentSlotType,
                opt => opt.MapFrom(src =>
                    HohStringParser.ParseEnumFromString<EquipmentSlotType>(src.EquipmentSlotTypeDefinitionId)))
            .ForMember(dest => dest.EquipmentRarity,
                opt => opt.ConvertUsing<EquipmentRarityValueConverter, string>(src => src.EquipmentRarityDefinitionId))
            .ForMember(dest => dest.EquipmentSet,
                opt => opt.MapFrom(src =>
                    HohStringParser.ParseEnumFromString<EquipmentSet>(src.EquipmentSetDefinitionId)));

        CreateMap<EquipmentAttributeDto, EquipmentAttribute>()
            .ForMember(dest => dest.StatAttribute,
                opt => opt.MapFrom(src =>
                    HohStringParser.ParseEnumFromString<StatAttribute>(src.UnitStatAttributeDefinitionId)));

        CreateMap<CityMapEntityDto, CityMapEntity>()
            .ForMember(dest => dest.CityEntityId,
                opt => opt.MapFrom(src => HohStringParser.GetConcreteId(src.CityEntityId)))
            .ForMember(dest => dest.CustomizationId, opt =>
            {
                opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.CustomizationEntityId));
                opt.MapFrom(src => HohStringParser.GetConcreteId(src.CustomizationEntityId));
            });
        CreateMap<CityDTO, City>()
            .ForMember(dest => dest.CityId, opt => opt.ConvertUsing(new CityIdValueConverter(), src => src.CityId))
            .ForMember(dest => dest.OpenedExpansions, opt => opt.MapFrom(src => src.ExpansionMapEntities));
        CreateMap<CityMapEntityProductionDto, CityMapEntityProduction>()
            .ForMember(dest => dest.DefinitionId,
                opt => opt.MapFrom(src => HohStringParser.GetConcreteId(src.DefinitionId)));
        CreateMap<ExpansionMapEntityDto, CityMapExpansion>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => HohStringParser.GetConcreteId(src.Id)));
        CreateMap<OtherCityDTO, OtherCity>()
            .ForMember(dest => dest.CityId, opt => opt.ConvertUsing(new CityIdValueConverter(), src => src.CityId))
            .ForMember(dest => dest.OpenedExpansions, opt => opt.MapFrom(src => src.ExpansionMapEntities))
            .ForMember(dest => dest.Wonders, opt =>
            {
                opt.PreCondition(src => src.Wonders != null && src.Wonders.Wonders.Count > 0);
                opt.MapFrom(src => src.Wonders!.Wonders);
            });

        CreateMap<StatBoostDto, StatBoost>()
            .ForMember(dest => dest.UnitStatType,
                opt => opt.MapFrom(src => HohStringParser.ParseEnumFromString<UnitStatType>(src.UnitStatDefinitionId)))
            .ForMember(dest => dest.StatAttribute, opt =>
            {
                opt.PreCondition(src => src.HasUnitStatAttributeDefinitionId);
                opt.MapFrom(src =>
                    HohStringParser.ParseEnumFromString<StatAttribute>(src.UnitStatAttributeDefinitionId));
            })
            .ForMember(dest => dest.Calculation, opt => opt.MapFrom(src => src.Calculation));

        CreateMap<PvpResultPointsDto, PvpResultPoints>();
        CreateMap<BattleUnitStateDto, BattleUnitState>()
            .ForMember(dest => dest.UnitStats,
                opt => opt.MapFrom(src =>
                    src.UnitStats.ToDictionary(kvp => HohStringParser.ParseEnumFromString<UnitStatType>(kvp.Key),
                        kvp => kvp.Value)));
        CreateMap<BattleUnitPropertiesDto, BattleUnitProperties>()
            .ForMember(dest => dest.UnitStatsOverrides,
                opt => opt.MapFrom(src =>
                    src.UnitStatsOverrides.ToDictionary(kvp =>
                        HohStringParser.ParseEnumFromString<UnitStatType>(kvp.Key), kvp => kvp.Value)))
            .ForMember(dest => dest.Equipment,
                opt =>
                {
                    opt.PreCondition(src => src.Equipment != null);
                    opt.MapFrom((src, _, _, context) =>
                        context.Mapper.Map<IReadOnlyCollection<SquadEquipmentItem>>(src.Equipment
                            .Unpack<AllEquipmentUnitDataDTO>().Items));
                });
        CreateMap<BattleUnitDto, BattleUnit>()
            .ForMember(dest => dest.UnitState, opt => opt.PreCondition(src => src.UnitState != null));
        CreateMap<BattleSquadDto, BattleSquad>()
            .ForMember(dest => dest.Hero, opt => opt.PreCondition(src => src.HasHero))
            .ForMember(dest => dest.Unit, opt => opt.PreCondition(src => src.HasUnit));
        CreateMap<CampaignMapBattleLocationDTO, CampaignMapBattleLocation>()
            .ForMember(dest => dest.Difficulty,
                opt => opt.MapFrom(src => HohStringParser.ParseEnumFromString<Difficulty>(src.Difficulty)));
        CreateMap<HistoricBattleLocationDTO, HistoricBattleLocation>()
            .ForMember(dest => dest.Difficulty,
                opt => opt.MapFrom(src => HohStringParser.ParseEnumFromString<Difficulty>(src.Difficulty)));
        CreateMap<HeroTreasureHuntEncounterLocationDTO, TreasureHuntEncounterLocation>();
        CreateMap<PvpBattleLocationDataDTO, PvpBattleLocation>()
            .ForMember(dest => dest.EnemyAlliance, opt => opt.PreCondition(src => src.EnemyAlliance != null));
        CreateMap<Any, BattleLocationBase>().ConvertUsing<BattleLocationDtoConverter>();
        CreateMap<BattleSummaryDto, BattleSummary>()
            .ForMember(dest => dest.BattleId, opt => opt.MapFrom(src => src.BattleId.ToByteArray()))
            .ForMember(dest => dest.ResultStatus, opt => opt.MapFrom(src => src.ResultStatus.Status))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.PackedEncounterLocation));
        CreateMap<ProfileSquadDto, ProfileSquad>();

        CreateMap<HeroBattleStatsResponse, BattleStats>();
        CreateMap<BattleSquadStatsDto, BattleSquadStats>();
        CreateMap<UnitBattleStatsDto, UnitBattleStats>();
        CreateMap<UnitBattleStatsSubValueDto, UnitBattleStatsSubValue>();

        CreateMap<byte[], BattleStatsRequestDto>()
            .ForMember(dest => dest.BattleId, opt => opt.MapFrom(src => ByteString.CopyFrom(src)));

        CreateMap<ReworkedWonderDto, CityWonder>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => HohStringParser.ParseEnumFromString2<WonderId>(src.Id, '_')));

        CreateMap<PlayerProfileResponse, PlayerProfile>()
            .ForMember(dest => dest.Player, opt => opt.MapFrom(src => src.PlayerWithAlliance))
            .ForMember(dest => dest.Alliance, opt =>
            {
                opt.PreCondition(src => src.PlayerWithAlliance.HasAllianceId);
                opt.MapFrom(src => src.PlayerWithAlliance);
            })
            .ForMember(dest => dest.PvpTier, opt =>
            {
                opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.PvpTier));
                opt.MapFrom(src => src.PvpTier);
            })
            .ForMember(dest => dest.TreasureHuntDifficulty, opt =>
            {
                opt.PreCondition(src => src.HasTreasureHuntDifficulty && src.TreasureHuntDifficulty >= 0);
                opt.MapFrom(src => src.TreasureHuntDifficulty);
            });
        CreateMap<PlayerWithAllianceDto, HohPlayer>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PlayerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PlayerName))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => HohStringParser.GetConcreteId(src.PlayerAge)))
            .ForMember(dest => dest.AvatarId, opt => opt.MapFrom(src => src.PlayerAvatarId));
        CreateMap<PlayerWithAllianceDto, HohAlliance>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AllianceId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AllianceName))
            .ForMember(dest => dest.AvatarIconId, opt => opt.MapFrom(src => src.AllianceAvatarIconId))
            .ForMember(dest => dest.AvatarBackgroundId, opt => opt.MapFrom(src => src.AllianceAvatarBackgroundId));

        CreateMap<ResearchStateTechnologyDto, ResearchStateTechnology>()
            .ForMember(dest => dest.TechnologyId,
                opt => opt.MapFrom(src => HohStringParser.GetConcreteId(src.TechnologyId)));

        CreateMap<PlayerWithAllianceDto, (HohPlayer Player, HohAlliance Alliance)>()
            .ForMember(dest => dest.Player, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Alliance, opt => opt.MapFrom(src => src));
        CreateMap<LeaderboardPush, Leaderboard>()
            .ForMember(dest => dest.Participants,
                opt => opt.MapFrom(src => src.Leaderboard.Participants.Select(x => x.Player)));
    }
}

using AutoMapper;
using Ingweland.Fog.Application.Server.Battle.Queries;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class BattleMappingProfile : Profile
{
    public BattleMappingProfile()
    {
        CreateMap<BattleSummaryEntity, BattleKey>();

        CreateMap<BattleSearchRequest, BattleSearchQuery>();
        CreateMap<BattleUnitProperties, BattleUnitDto>();
        CreateMap<BattleUnit, BattleUnitDto>()
            .ForMember(dest => dest.UnitId, opt => opt.MapFrom(x => x.Properties.UnitId))
            .ForMember(dest => dest.AbilityLevel,
                opt => opt.MapFrom(x => x.Properties.AbilityLevel > 0 ? x.Properties.AbilityLevel : 1))
            .ForMember(dest => dest.Abilities, opt => opt.MapFrom(x => x.Properties.Abilities))
            .ForMember(dest => dest.AscensionLevel, opt => opt.MapFrom(x => x.Properties.AscensionLevel))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(x => x.Properties.Level))
            .ForMember(dest => dest.StatBoosts, opt => opt.MapFrom(x => x.Properties.StatBoosts))
            .ForMember(dest => dest.UnitStatsOverrides, opt => opt.MapFrom(x => x.Properties.UnitStatsOverrides))
            .ForMember(dest => dest.FinalState, opt =>
            {
                opt.PreCondition(x => x.UnitState != null);
                opt.MapFrom(x => x.UnitState!.UnitStats);
            })
            .ForMember(dest => dest.UnitInBattleId, opt =>
            {
                opt.PreCondition(x => x.UnitState != null);
                opt.MapFrom(x => x.UnitState!.InBattleId);
            });

        CreateMap<BattleTimelineEntry, BattleTimelineEntryDto>()
            .ForMember(dest => dest.AbilityId, opt => opt.MapFrom(x => x.AbilityId))
            .ForMember(dest => dest.TimeSeconds, opt => opt.MapFrom(x => x.TimeMillis / 1000));

        CreateMap<BattleSquad, BattleSquadDto>();
    }
}

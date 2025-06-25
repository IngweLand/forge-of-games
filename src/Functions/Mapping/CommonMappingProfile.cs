using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Functions.Mapping;

public class CommonMappingProfile:Profile
{
    public CommonMappingProfile()
    {
        CreateMap<PlayerRankingType, Models.Hoh.Enums.PlayerRankingType>().ReverseMap();
        CreateMap<AllianceRankingType, Models.Hoh.Enums.AllianceRankingType>().ReverseMap();
        
        CreateMap<BattleSquadStats, BattleSquadStatsEntity>()
            .ForMember(dest => dest.Hero, opt =>
            {
                opt.PreCondition(src => src.Hero != null);
                opt.MapFrom(src => src.Hero);
            })
            .ForMember(dest => dest.SupportUnit, opt =>
            {
                opt.PreCondition(src => src.SupportUnit != null);
                opt.MapFrom(src => src.SupportUnit);
            });
        CreateMap<UnitBattleStats, UnitBattleStatsEntity>();
        CreateMap<UnitBattleStatsSubValue, UnitBattleStatsEntity>();
    }
}

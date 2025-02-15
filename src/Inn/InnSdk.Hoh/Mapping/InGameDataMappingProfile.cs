using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;

namespace Ingweland.Fog.InnSdk.Hoh.Mapping;

public class InGameDataMappingProfile : Profile
{
    public InGameDataMappingProfile()
    {
        CreateMap<PlayerRankDto, PlayerRank>()
            .ForMember(dest => dest.AllianceName, opt => opt.PreCondition(src => src.HasAllianceName));
        CreateMap<PlayerRanksDTO, PlayerRanks>();
        CreateMap<AllianceRankDto, AllianceRank>();
        CreateMap<AllianceRanksDTO, AllianceRanks>();
    }
}

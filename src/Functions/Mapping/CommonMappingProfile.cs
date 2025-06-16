using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;

namespace Ingweland.Fog.Functions.Mapping;

public class CommonMappingProfile:Profile
{
    public CommonMappingProfile()
    {
        CreateMap<PlayerRankingType, Models.Hoh.Enums.PlayerRankingType>().ReverseMap();
        CreateMap<AllianceRankingType, Models.Hoh.Enums.AllianceRankingType>().ReverseMap();
    }
}

using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;

namespace Ingweland.Fog.InnSdk.Hoh.Mapping;

public class EnumMappingProfile:Profile
{
    public EnumMappingProfile()
    {
        CreateMap<BattleResultStatus, Models.Hoh.Enums.BattleResultStatus>().ReverseMap();
    }
}

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
        CreateMap<BattleUnitProperties, BattleSquadDto>();
    }
}

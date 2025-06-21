using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.InnSdk.Hoh.Mapping.Converters;

public class BattleLocationDtoConverter : ITypeConverter<Any, BattleLocationBase>
{
    public BattleLocationBase Convert(Any source, BattleLocationBase destination, ResolutionContext context)
    {
        var mapper = context.Mapper;

        if (source.Is(CampaignMapBattleLocationDTO.Descriptor))
        {
            return mapper.Map<CampaignMapBattleLocation>(source.Unpack<CampaignMapBattleLocationDTO>());
        }

        if (source.Is(HeroTreasureHuntEncounterLocationDTO.Descriptor))
        {
            return mapper.Map<TreasureHuntEncounterLocation>(source.Unpack<HeroTreasureHuntEncounterLocationDTO>());
        }

        if (source.Is(PvpBattleLocationDataDTO.Descriptor))
        {
            return mapper.Map<PvpBattleLocation>(source.Unpack<PvpBattleLocationDataDTO>());
        }
        
        if (source.Is(HistoricBattleLocationDTO.Descriptor))
        {
            return mapper.Map<HistoricBattleLocation>(source.Unpack<HistoricBattleLocationDTO>());
        }

        throw new Exception($"Unknown location type: {source.TypeUrl}");
    }
}

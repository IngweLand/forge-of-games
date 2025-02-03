using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace HohProtoParser.Converters;

public class HeroBattleDefinitionDtoConverter:ITypeConverter<HeroBattleDefinitionDTO, BattleDetails>
{
    public BattleDetails Convert(HeroBattleDefinitionDTO source, BattleDetails destination, ResolutionContext context)
    {
        var battleWaves = (IList<HeroBattleWaveDefinitionDTO>)context.Items[ContextKeys.BATTLE_WAVES_DEFINITIONS];
        var waves = battleWaves.Where(d => source.Waves.Contains(d.Id));
        return new BattleDetails()
        {
            Id = source.Id,
            Waves = context.Mapper.Map<IReadOnlyCollection<BattleWave>>(waves),
            DisabledPlayerSlotIds = context.Mapper.Map<IReadOnlyCollection<int>>(source.DisabledPlayerSlotIds),
        };
    }
}

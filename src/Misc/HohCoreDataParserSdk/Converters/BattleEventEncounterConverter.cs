using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.HohCoreDataParserSdk.Converters;

public class BattleEventEncounterConverter : ITypeConverter<EncounterDefinitionDTO, BattleEventEncounter>
{
    public BattleEventEncounter Convert(EncounterDefinitionDTO source, BattleEventEncounter destination,
        ResolutionContext context)
    {
        var battles = (IList<HeroBattleDefinitionDTO>) context.Items[ContextKeys.BATTLES_DEFINITIONS];

        return new BattleEventEncounter
        {
            Id = source.EncounterId,
            Index = int.Parse(HohStringParser.GetConcreteId(source.EncounterId, '_')),
            BattleDetails = context.Mapper.Map<BattleDetails>(battles.Single(hbd => hbd.Id == source.HeroBattleId)),
        };
    }
}

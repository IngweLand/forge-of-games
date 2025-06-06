using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Helpers;

namespace HohProtoParser.Converters;

public class EncounterDefinitionDtoConverter : ITypeConverter<EncounterDefinitionDTO, Encounter>
{
    public Encounter Convert(EncounterDefinitionDTO source, Encounter destination, ResolutionContext context)
    {
        context.Items[ContextKeys.HERO_BATTLE_ID] = source.HeroBattleId;

        return new Encounter()
        {
            Id = source.EncounterId,
            Index = int.Parse(HohStringParser.GetConcreteId(source.EncounterId, '_')),
            Details = context.Mapper.Map<IReadOnlyDictionary<Difficulty, EncounterDetails>>(source.Encounters),
        };
    }
}

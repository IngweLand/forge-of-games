using AutoMapper;
using HohProtoParser.Helpers;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Converters;

public class EncounterDefinitionDtoConverter:ITypeConverter<EncounterDefinitionDTO, Encounter>
{
    public Encounter Convert(EncounterDefinitionDTO source, Encounter destination, ResolutionContext context)
    {
        context.Items[ContextKeys.HERO_BATTLE_ID] = source.HeroBattleId;
        
        return new Encounter()
        {
            Id = source.EncounterId,
            Index = int.Parse(StringParser.GetConcreteId(source.EncounterId, '_')),
            Details = context.Mapper.Map<IReadOnlyDictionary<Difficulty, EncounterDetails>>(source.Encounters),
        };
    }
}

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
        var battles = (IList<HeroBattleDefinitionDTO>)context.Items[ContextKeys.BATTLES_DEFINITIONS];
        return new Encounter()
        {
            Id = source.EncounterId,
            Index = int.Parse(StringParser.GetConcreteId(source.EncounterId, '_')),
            Difficulty = StringParser.ParseEnumFromString<Difficulty>(source.Encounter.Difficulty),
            Cost = context.Mapper.Map<IReadOnlyCollection<ResourceAmount>>(source.Encounter.Details.Cost.Resources),
            AutoVictoryCost =
                context.Mapper.Map<IReadOnlyCollection<ResourceAmount>>(source.Encounter.Details.AutoVictoryCost.Resources),
            Rewards = context.Mapper.Map<IReadOnlyCollection<RewardBase>>(source.Encounter.Details.RegularReward
                .PackedRewards),
            FirstTimeCompletionBonus =
                context.Mapper.Map<IReadOnlyCollection<RewardBase>>(
                    source.Encounter.Details.FirstVictoryBonus.PackedRewards).OfType<ResourceReward>().ToList().AsReadOnly(),
            BattleDetails = context.Mapper.Map<BattleDetails>(battles.Single(hbd=> hbd.Id == source.HeroBattleId)),
        };
    }
}

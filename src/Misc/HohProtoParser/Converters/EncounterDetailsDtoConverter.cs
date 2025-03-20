using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Converters;

public class EncounterDetailsDtoConverter : ITypeConverter<EncounterDetailsDto, EncounterDetails>
{
    public EncounterDetails Convert(EncounterDetailsDto source, EncounterDetails destination, ResolutionContext context)
    {
        var battles = (IList<HeroBattleDefinitionDTO>) context.Items[ContextKeys.BATTLES_DEFINITIONS];
        var difficulty = (Difficulty) context.Items[ContextKeys.DIFFICULTY];
        var heroBattleId = difficulty == Difficulty.Hard
            ? source.HeroBattleId
            : (string) context.Items[ContextKeys.HERO_BATTLE_ID];
        return new EncounterDetails()
        {
            Cost = context.Mapper.Map<IReadOnlyCollection<ResourceAmount>>(source.Cost.Resources),
            AutoVictoryCost =
                context.Mapper.Map<IReadOnlyCollection<ResourceAmount>>(source.AutoVictoryCost.Resources),
            Rewards = context.Mapper.Map<IReadOnlyCollection<RewardBase>>(source.RegularReward.PackedRewards),
            FirstTimeCompletionBonus =
                context.Mapper.Map<IReadOnlyCollection<RewardBase>>(
                    source.FirstVictoryBonus.PackedRewards).OfType<ResourceReward>().ToList().AsReadOnly(),
            BattleDetails = string.IsNullOrWhiteSpace(heroBattleId)
                ? null
                : context.Mapper.Map<BattleDetails>(battles.Single(hbd => hbd.Id == heroBattleId)),
        };
    }
}
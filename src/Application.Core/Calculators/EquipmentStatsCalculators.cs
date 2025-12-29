using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Calculators;

public class EquipmentStatsCalculators : IEquipmentStatsCalculators
{
    public IReadOnlyDictionary<UnitStatType, float> Calculate(EquipmentItem? hand, EquipmentItem? garment,
        EquipmentItem? hat, EquipmentItem? neck, EquipmentItem? ring,
        IReadOnlyDictionary<UnitStatType, float> heroBaseStats,
        IReadOnlyDictionary<EquipmentSet, EquipmentSetDefinition> setDefinitions)
    {
        var statBoosts = new List<StatBoost>();
        AddStatBoosts(statBoosts, hand);
        AddStatBoosts(statBoosts, garment);
        AddStatBoosts(statBoosts, hat);
        AddStatBoosts(statBoosts, neck);
        AddStatBoosts(statBoosts, ring);
        if (hand != null && hand.EquipmentSet == garment?.EquipmentSet)
        {
            var setDefinition = setDefinitions[hand.EquipmentSet];
            statBoosts.AddRange(setDefinition.BonusBoosts);
        }

        if (hat != null && hat.EquipmentSet == neck?.EquipmentSet &&
            hat.EquipmentSet == ring?.EquipmentSet)
        {
            var setDefinition = setDefinitions[hat.EquipmentSet];
            statBoosts.AddRange(setDefinition.BonusBoosts);
        }

        var boostedStats = new Dictionary<UnitStatType, float>();
        foreach (var statBoost in statBoosts)
        {
            boostedStats.TryGetValue(statBoost.UnitStatType, out var boostedValue);
            var baseValue = heroBaseStats[statBoost.UnitStatType];
            switch (statBoost.Calculation)
            {
                case Calculation.Add:
                    boostedValue += statBoost.Value;
                    break;
                case Calculation.Multiply:
                    boostedValue += baseValue * statBoost.Value;
                    break;
            }

            boostedStats[statBoost.UnitStatType] = boostedValue;
        }

        return boostedStats;
    }

    private static void AddStatBoosts(List<StatBoost> statBoosts, EquipmentItem? item)
    {
        if (item == null)
        {
            return;
        }

        if (item.MainAttribute.StatBoost != null)
        {
            statBoosts.Add(item.MainAttribute.StatBoost);
        }

        statBoosts.AddRange(item.SubAttributes
            .Where(x => x.StatBoost != null)
            .Select(x => x.StatBoost!));
    }
}

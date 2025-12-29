using System.Globalization;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Factories;

public class UnitStatsViewModelFactory(IAssetUrlProvider assetUrlProvider) : IUnitStatsViewModelFactory
{
    private const int DEFAULT_HITS_PER_MINUTE = 60;

    private static readonly List<UnitStatType> MainDisplayedStats =
    [
        UnitStatType.Attack,
        UnitStatType.Defense,
        UnitStatType.MaxHitPoints,
        UnitStatType.BaseDamage,
    ];

    private static readonly List<UnitStatType> DisplayedStats =
    [
        UnitStatType.Attack,
        UnitStatType.Defense,
        UnitStatType.MaxHitPoints,
        UnitStatType.BaseDamage,
        UnitStatType.AttackSpeed,
        UnitStatType.Evasion,
        UnitStatType.CritChance,
        UnitStatType.CritDamage,
        UnitStatType.SingleTargetDamageAmp,
        UnitStatType.AoeDamageAmp,
        UnitStatType.BasicAttackDamageAmp,
        UnitStatType.HealTakenAmp,
    ];

    private static readonly HashSet<UnitStatType> PercentageBasedStats =
    [
        UnitStatType.CritChance,
        UnitStatType.CritDamage,
        UnitStatType.SingleTargetDamageAmp,
        UnitStatType.HealTakenAmp,
        UnitStatType.Evasion,
    ];

    private static readonly List<UnitStatType> EquipmentStats =
    [
        UnitStatType.Attack,
        UnitStatType.Defense,
        UnitStatType.MaxHitPoints,
        UnitStatType.InitialFocusInSecondsBonus,
        UnitStatType.SingleTargetDamageAmp,
        UnitStatType.AoeDamageAmp,
        UnitStatType.DotDamageAmp,
        UnitStatType.HealGivenAmp,
        UnitStatType.BaseDamage,
        UnitStatType.BasicAttackDamageAmp,
        UnitStatType.AttackSpeed,
        UnitStatType.CritChance,
        UnitStatType.CritDamage,
        UnitStatType.Evasion,
        UnitStatType.HealTakenAmp,
        UnitStatType.ShieldTakenAmp,
    ];

    private Dictionary<UnitStatType, NumericValueType> StatToNumericValueTypeMap { get; } = new()
    {
        {UnitStatType.Focus, NumericValueType.Duration},
        {UnitStatType.InitialFocusInSecondsBonus, NumericValueType.Duration},
        {UnitStatType.FocusRegen, NumericValueType.Duration},
        {UnitStatType.MaxFocus, NumericValueType.Duration},
        {UnitStatType.SingleTargetDamageAmp, NumericValueType.Percentage},
        {UnitStatType.AoeDamageAmp, NumericValueType.Percentage},
        {UnitStatType.DotDamageAmp, NumericValueType.Percentage},
        {UnitStatType.HealGivenAmp, NumericValueType.Percentage},
        {UnitStatType.BasicAttackDamageAmp, NumericValueType.Percentage},
        {UnitStatType.AttackSpeed, NumericValueType.Speed},
        {UnitStatType.CritChance, NumericValueType.Percentage},
        {UnitStatType.CritDamage, NumericValueType.Percentage},
        {UnitStatType.Evasion, NumericValueType.Percentage},
        {UnitStatType.HealTakenAmp, NumericValueType.Percentage},
        {UnitStatType.ShieldTakenAmp, NumericValueType.Percentage},
    };

    public IReadOnlyCollection<IconLabelItemViewModel> CreateMainStatsItems(
        IReadOnlyDictionary<UnitStatType, float> stats)
    {
        var list = new List<IconLabelItemViewModel>();
        foreach (var stat in MainDisplayedStats)
        {
            if (!stats.TryGetValue(stat, out var value))
            {
                continue;
            }

            list.Add(new IconLabelItemViewModel
            {
                IconUrl = assetUrlProvider.GetHohUnitStatIconUrl(stat),
                Label = UnitStatToString(stat, value),
            });
        }

        return list;
    }

    public IReadOnlyCollection<UnitStatBreakdownViewModel> CreateStatsBreakdownItems(
        IReadOnlyDictionary<UnitStatType, IReadOnlyDictionary<UnitStatSource, float>> stats)
    {
        var list = new List<UnitStatBreakdownViewModel>();
        foreach (var stat in EquipmentStats)
        {
            if (!stats.TryGetValue(stat, out var breakdown) || breakdown.Count == 0)
            {
                continue;
            }

            list.Add(new UnitStatBreakdownViewModel
            {
                IconUrl = assetUrlProvider.GetHohUnitStatIconUrl(stat),
                Values = breakdown.ToDictionary(kvp => kvp.Key, kvp => UnitStatToString(stat, kvp.Value)),
                TotalValue = UnitStatToString(stat, breakdown.Values.Sum(v => v)),
            });
        }

        return list;
    }

    public IReadOnlyCollection<IconLabelsItemViewModel> CreateStatsItems(IReadOnlyDictionary<UnitStatType, float> stats,
        IReadOnlyDictionary<UnitStatType, string> unitStatNames)
    {
        return stats
            .OrderBy(kvp => GetSortOrder(kvp.Key))
            .Select(kvp =>
            {
                var numericValueType =
                    StatToNumericValueTypeMap.GetValueOrDefault(kvp.Key, NumericValueType.Number);
                var name = unitStatNames.TryGetValue(kvp.Key, out var set) ? set : kvp.Key.ToString();
                return new IconLabelsItemViewModel
                {
                    IconUrl = assetUrlProvider.GetHohUnitStatIconUrl(kvp.Key),
                    Label = name,
                    Label2 = numericValueType.ToFormatedString2(kvp.Value),
                };
            })
            .ToList();
    }

    private int GetSortOrder(UnitStatType unitStat)
    {
        var i = DisplayedStats.IndexOf(unitStat);
        return i == -1 ? int.MaxValue : i;
    }

    private static string UnitStatToString(UnitStatType stat, float value)
    {
        string label;
        if (stat == UnitStatType.AttackSpeed)
        {
            label = Math.Round(DEFAULT_HITS_PER_MINUTE * value, MidpointRounding.AwayFromZero)
                .ToString(CultureInfo.InvariantCulture);
        }
        else if (PercentageBasedStats.Contains(stat))
        {
            label = value.ToString("P1");
        }
        else
        {
            label = Math.Round(value, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
        }

        return label;
    }
}

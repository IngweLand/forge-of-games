using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class EquipmentItemAttributeViewModelFactory(IAssetUrlProvider assetUrlProvider)
    : IEquipmentItemAttributeViewModelFactory
{
    public Dictionary<StatAttribute, NumericValueType> StatToNumericValueTypeMap { get; } = new()
    {
        {StatAttribute.Attack, NumericValueType.Number},
        {StatAttribute.AttackBonus, NumericValueType.Percentage},
        {StatAttribute.Defense, NumericValueType.Number},
        {StatAttribute.DefenseBonus, NumericValueType.Percentage},
        {StatAttribute.MaxHitPoints, NumericValueType.Number},
        {StatAttribute.MaxHitPointsBonus, NumericValueType.Percentage},
        {StatAttribute.BaseDamageBonus, NumericValueType.Percentage},
        {StatAttribute.CritDamage, NumericValueType.Percentage},
        {StatAttribute.InitialFocusInSecondsBonus, NumericValueType.Duration},
        {StatAttribute.CritChance, NumericValueType.Percentage},
        {StatAttribute.AttackSpeed, NumericValueType.Speed},
    };

    public EquipmentItemAttributeViewModel CreateAttribute(EquipmentAttribute attribute)
    {
        var numericValueType =
            StatToNumericValueTypeMap.GetValueOrDefault(attribute.StatAttribute, NumericValueType.Number);

        var formattedValue = numericValueType.ToFormatedString(attribute.StatBoost!.Value);

        return new EquipmentItemAttributeViewModel
        {
            Value = attribute.StatBoost.Value,
            FormattedValue = formattedValue,
            StatAttribute = attribute.StatAttribute,
        };
    }

    public EquipmentItemSubAttributeViewModel? CreateSubAttribute(IEnumerable<EquipmentAttribute> attributes,
        StatAttribute targetAttribute)
    {
        var attribute = attributes.FirstOrDefault(ea => ea.StatAttribute == targetAttribute);

        if (attribute == null)
        {
            return null;
        }

        var formattedValue = "?";
        if (attribute.StatBoost != null)
        {
            var numericValueType =
                StatToNumericValueTypeMap.GetValueOrDefault(attribute.StatAttribute, NumericValueType.Number);
            formattedValue = numericValueType.ToFormatedString(attribute.StatBoost.Value);
        }

        formattedValue += $"<sup style=\"font-size: 8px;\"> lvl {attribute.UnlockedAtLevel}</sup>";

        return new EquipmentItemSubAttributeViewModel
        {
            Value = attribute.StatBoost?.Value,
            FormattedValue = formattedValue,
            UnlockedAtLevel = attribute.UnlockedAtLevel,
            Unlocked = attribute.Unlocked,
            RolledValuePercent = attribute.Unlocked ? attribute.RolledValue * 100 : 0,
            FormattedRolledValue = attribute.Unlocked ? attribute.RolledValue.ToString("P1") : null,
        };
    }

    public IconLabelsItemViewModel CreateAttribute(EquipmentAttribute attribute,
        IReadOnlyDictionary<StatAttribute, string> statAttributeNames)
    {
        var numericValueType =
            StatToNumericValueTypeMap.GetValueOrDefault(attribute.StatAttribute, NumericValueType.Number);

        var formattedValue = numericValueType.ToFormatedString(attribute.StatBoost!.Value);
        var name = statAttributeNames.TryGetValue(attribute.StatAttribute, out var set) ? set : attribute.ToString();

        return new IconLabelsItemViewModel
        {
            IconUrl = assetUrlProvider.GetHohStatAttributeIconUrl(attribute.StatAttribute),
            Label = name!,
            Label2 = formattedValue,
        };
    }

    public EquipmentItemSubAttributeViewModel2 CreateSubAttribute(EquipmentAttribute attribute,
        IReadOnlyDictionary<StatAttribute, string> statAttributeNames)
    {
        var formattedValue = "?";
        if (attribute.StatBoost != null)
        {
            var numericValueType =
                StatToNumericValueTypeMap.GetValueOrDefault(attribute.StatAttribute, NumericValueType.Number);
            formattedValue = numericValueType.ToFormatedString(attribute.StatBoost.Value);
        }

        var name = statAttributeNames.TryGetValue(attribute.StatAttribute, out var set) ? set : attribute.ToString();

        return new EquipmentItemSubAttributeViewModel2
        {
            IconUrl = assetUrlProvider.GetHohStatAttributeIconUrl(attribute.StatAttribute),
            Label = name!,
            Label2 = formattedValue,
            RolledValuePercent = attribute.Unlocked ? attribute.RolledValue * 100 : 0,
            FormattedRolledValue = attribute.Unlocked ? attribute.RolledValue.ToString("P1") : null,
        };
    }
}

using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class EquipmentItemToViewModelConverter(
    IAssetUrlProvider assetUrlProvider,
    IEquipmentSlotTypeIconUrlProvider equipmentSlotTypeIconUrlProvider)
    : ITypeConverter<EquipmentItem, EquipmentItemViewModel>
{
    private static readonly Dictionary<StatAttribute, NumericValueType> StatToNumericValueTypeMap = new()
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

    public EquipmentItemViewModel Convert(EquipmentItem source, EquipmentItemViewModel destination,
        ResolutionContext context)
    {
        var heroAssetId = source.EquippedOnHero != null ? $"Unit_{source.EquippedOnHero}" : null;

        var subAttributes = StatToNumericValueTypeMap.Keys.ToDictionary(statAttribute => statAttribute,
            statAttribute => CreateSubAttribute(source.SubAttributes, statAttribute));
        return new EquipmentItemViewModel
        {
            Id = source.Id,
            EquipmentSlotType = source.EquipmentSlotType,
            EquipmentSet = source.EquipmentSet,
            Level = source.Level,
            StarCount = source.EquipmentRarity.ToStarCount(),
            EquippedOnHero = source.EquippedOnHero,
            EquippedOnHeroPortraitUrl =
                heroAssetId != null ? assetUrlProvider.GetHohUnitPortraitUrl(heroAssetId) : null,
            EquipmentSetIconUrl = assetUrlProvider.GetHohEquipmentSetIconUrl(source.EquipmentSet),
            EquipmentSlotTypeIconUrl = equipmentSlotTypeIconUrlProvider.GetIconUrl(source.EquipmentSlotType),
            MainAttribute = CreateAttribute(source.MainAttribute),
            SubAttributes = subAttributes,
        };
    }

    private static EquipmentItemAttributeViewModel CreateAttribute(EquipmentAttribute attribute)
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

    private static EquipmentItemSubAttributeViewModel? CreateSubAttribute(IEnumerable<EquipmentAttribute> attributes,
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
}

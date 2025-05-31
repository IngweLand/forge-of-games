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
    public EquipmentItemViewModel Convert(EquipmentItem source, EquipmentItemViewModel destination,
        ResolutionContext context)
    {
        var heroAssetId = source.EquippedOnHero != null ? $"Unit_{source.EquippedOnHero}" : null;

        return new EquipmentItemViewModel()
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
            MainAttack = CreateAttribute(source.MainAttribute, StatAttribute.Attack),
            MainDefense = CreateAttribute(source.MainAttribute, StatAttribute.Defense),
            SubAttack = CreateSubAttribute(source.SubAttributes, StatAttribute.Attack),
            SubAttackAmp = CreateSubAttribute(source.SubAttributes, StatAttribute.AttackBonus),
            SubDefense = CreateSubAttribute(source.SubAttributes, StatAttribute.Defense),
            SubDefenseAmp = CreateSubAttribute(source.SubAttributes, StatAttribute.DefenseBonus),
            SubHitPoints = CreateSubAttribute(source.SubAttributes, StatAttribute.MaxHitPoints),
            SubHitPointsAmp = CreateSubAttribute(source.SubAttributes, StatAttribute.MaxHitPointsBonus),
            SubBaseDamageAmp = CreateSubAttribute(source.SubAttributes, StatAttribute.BaseDamageBonus),
            SubCritDamage = CreateSubAttribute(source.SubAttributes, StatAttribute.CritDamage),
        };
    }

    private static EquipmentItemAttributeViewModel? CreateAttribute(EquipmentAttribute attribute, StatAttribute targetAttribute)
    {
        if (attribute.StatBoost == null || attribute.StatAttribute != targetAttribute)
        {
            return null;
        }

        var formattedValue = attribute.StatBoost.Calculation switch
        {
            Calculation.Multiply => (attribute.StatBoost.Value * 100).ToString("N1"),
            _ => attribute.StatBoost.Value.ToString()
        };

        return new EquipmentItemAttributeViewModel()
        {
            Value = attribute.StatBoost.Value,
            FormattedValue = formattedValue,
        };
    }
    
    private static EquipmentItemSubAttributeViewModel? CreateSubAttribute(IEnumerable<EquipmentAttribute> attributes, StatAttribute targetAttribute)
    {
        var attribute = attributes.FirstOrDefault(ea => ea.StatAttribute == targetAttribute);
        
        if (attribute == null)
        {
            return null;
        }

        var formattedValue = "?";
        if (attribute.StatBoost != null)
        {
            formattedValue = attribute.StatBoost.Calculation switch
            {
                Calculation.Multiply => (attribute.StatBoost.Value * 100).ToString("N1"),
                _ => attribute.StatBoost.Value.ToString()
            };
        }

        formattedValue += $"<sup style=\"font-size: 8px;\"> lvl {attribute.UnlockedAtLevel}</sup>";

        return new EquipmentItemSubAttributeViewModel()
        {
            Value = attribute.StatBoost?.Value,
            FormattedValue = formattedValue,
            UnlockedAtLevel = attribute.UnlockedAtLevel,
            Unlocked = attribute.Unlocked
        };
    }
}
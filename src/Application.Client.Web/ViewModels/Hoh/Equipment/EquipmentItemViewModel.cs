using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;

public class EquipmentItemViewModel
{
    public EquipmentSet EquipmentSet { get; init; }
    public required string EquipmentSetIconUrl { get; init; }
    public EquipmentSlotType EquipmentSlotType { get; init; }
    public required string EquipmentSlotTypeIconUrl { get; init; }
    public string? EquippedOnHero { get; init; }
    public string? EquippedOnHeroPortraitUrl { get; init; }
    public int Id { get; init; }
    public int Level { get; init; }

    public EquipmentItemAttributeViewModel? MainAttack { get; init; }
    public EquipmentItemAttributeViewModel? MainDefense { get; init; }
    public int StarCount { get; init; }
    public EquipmentItemSubAttributeViewModel? SubAttack { get; init; }
    public EquipmentItemSubAttributeViewModel? SubAttackAmp { get; init; }
    public EquipmentItemSubAttributeViewModel? SubBaseDamageAmp { get; init; }
    public EquipmentItemSubAttributeViewModel? SubCritDamage { get; init; }

    public EquipmentItemSubAttributeViewModel? SubDefense { get; init; }
    public EquipmentItemSubAttributeViewModel? SubDefenseAmp { get; init; }
    public EquipmentItemSubAttributeViewModel? SubHitPoints { get; init; }
    public EquipmentItemSubAttributeViewModel? SubHitPointsAmp { get; init; }
}

public class EquipmentItemAttributeViewModel
{
    public string? FormattedValue { get; init; }
    public float Value { get; init; }
}

public class EquipmentItemSubAttributeViewModel
{
    public string? FormattedRolledValue { get; init; }
    public string? FormattedValue { get; init; }
    public float RolledValuePercent { get; init; }
    public bool Unlocked { get; set; }
    public int UnlockedAtLevel { get; init; }
    public float? Value { get; init; }
}

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

    public required EquipmentItemAttributeViewModel MainAttribute { get; init; }
    public int StarCount { get; init; }
    public required IReadOnlyDictionary<StatAttribute, EquipmentItemSubAttributeViewModel?> SubAttributes { get; init; }
}

public class EquipmentItemAttributeViewModel : IComparable<EquipmentItemAttributeViewModel>, IComparable
{
    public string? FormattedValue { get; init; }
    public required StatAttribute StatAttribute { get; init; }
    public float Value { get; init; }

    public int CompareTo(object? obj)
    {
        if (obj is EquipmentItemAttributeViewModel other)
        {
            return CompareTo(other);
        }

        return obj is null ? 1 : -1;
    }

    public int CompareTo(EquipmentItemAttributeViewModel? other)
    {
        return other is null ? 1 : Value.CompareTo(other.Value);
    }
}

public class EquipmentItemSubAttributeViewModel : IComparable<EquipmentItemSubAttributeViewModel>, IComparable
{
    public string? FormattedRolledValue { get; init; }
    public string? FormattedValue { get; init; }
    public float RolledValuePercent { get; init; }
    public bool Unlocked { get; set; }
    public int UnlockedAtLevel { get; init; }
    public float? Value { get; init; }

    public int CompareTo(object? obj)
    {
        if (obj is EquipmentItemSubAttributeViewModel other)
        {
            return CompareTo(other);
        }

        return obj is null ? 1 : -1;
    }

    public int CompareTo(EquipmentItemSubAttributeViewModel? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (Value is null && other.Value is null)
        {
            return 0;
        }

        if (Value is null)
        {
            return -1;
        }

        if (other.Value is null)
        {
            return 1;
        }

        return Value.Value.CompareTo(other.Value.Value);
    }
}

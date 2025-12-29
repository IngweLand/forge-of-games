namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;

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
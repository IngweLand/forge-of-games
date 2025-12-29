using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;

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
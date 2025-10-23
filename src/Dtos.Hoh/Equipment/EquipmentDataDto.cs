using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Equipment;

public class EquipmentDataDto
{
    public required IReadOnlyDictionary<EquipmentSet, string> Sets { get; init; }
    public required IReadOnlyDictionary<EquipmentSlotType, string> SlotTypes { get; init; }
    public required IReadOnlyDictionary<StatAttribute, string> StatAttributes { get; init; }
}

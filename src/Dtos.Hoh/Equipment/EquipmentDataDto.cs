using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Equipment;

public class EquipmentDataDto
{
    public required IReadOnlyDictionary<EquipmentSet, EquipmentSetDefinition> SetDefinitions { get; init; }
    public required IReadOnlyDictionary<string, string> SetNames { get; init; }
    public required IReadOnlyDictionary<EquipmentSlotType, string> SlotTypeNames { get; init; }
    public required IReadOnlyDictionary<StatAttribute, string> StatAttributeNames { get; init; }
    public required IReadOnlyDictionary<UnitStatType, string> UnitStatNames { get; init; }
}

using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Equipment;

[ProtoContract]
public class EquipmentSetDefinition
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<StatBoost> BonusBoosts { get; init; }
    [ProtoMember(2)]
    public required EquipmentSlotGroup Group { get; init; }
    [ProtoMember(3)]
    public required EquipmentSet Id { get; init; }
}

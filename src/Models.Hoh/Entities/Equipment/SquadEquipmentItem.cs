using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Equipment;

[ProtoContract]
public class SquadEquipmentItem
{
    [ProtoMember(1)]
    public EquipmentRarity EquipmentRarity { get; init; }

    [ProtoMember(2)]
    public EquipmentSet EquipmentSet { get; init; }

    [ProtoMember(3)]
    public EquipmentSlotType EquipmentSlotType { get; init; }

    [ProtoMember(4)]
    public int Level { get; init; }

    [ProtoMember(5)]
    public required EquipmentAttribute MainAttribute { get; init; }

    [ProtoMember(6)]
    public IReadOnlyCollection<EquipmentAttribute> SubAttributes { get; set; } = new List<EquipmentAttribute>();
}

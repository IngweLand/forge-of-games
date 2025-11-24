using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Equipment;

[ProtoContract]
public class EquipmentAttribute
{
    [ProtoMember(1)]
    public float RolledValue { get; init; }

    [ProtoMember(2)]
    public StatAttribute StatAttribute { get; init; }

    [ProtoMember(3)]
    public StatBoost? StatBoost { get; init; }

    [ProtoMember(4)]
    public bool Unlocked { get; init; }

    [ProtoMember(5)]
    public int UnlockedAtLevel { get; init; }
}

using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.Equipment;

public class EquipmentAttribute
{
    public StatAttribute StatAttribute { get; init; }
    public int UnlockedAtLevel { get; init; }
    public bool Unlocked { get; init; }
    public float RolledValue { get; init; }
    public EquipmentStatBoost? StatBoost { get; init; }
}
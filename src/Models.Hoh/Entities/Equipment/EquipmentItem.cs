using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.Equipment;

public class EquipmentItem
{
    public int Id { get; init; }
    public EquipmentSlotType EquipmentSlotType { get; init; }
    public EquipmentSet EquipmentSet { get; init; }
    public EquipmentRarity EquipmentRarity { get; init; }
    public int Level { get; init; }
    public string? EquippedOnHero { get; init; }
    public required EquipmentAttribute MainAttribute { get; init; }
    public IReadOnlyCollection<EquipmentAttribute> SubAttributes { get; set; } = new List<EquipmentAttribute>();
}
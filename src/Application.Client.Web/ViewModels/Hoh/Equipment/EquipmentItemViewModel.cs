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
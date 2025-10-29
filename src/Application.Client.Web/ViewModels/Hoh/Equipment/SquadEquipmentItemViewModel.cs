using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;

public class SquadEquipmentItemViewModel
{
    public required IReadOnlyCollection<IconLabelItemViewModel> Attributes { get; init; }
    public required IconLabelItemViewModel EquipmentSet { get; init; }
    public required EquipmentSlotType EquipmentSlotType { get; init; }
    public required string EquipmentSlotTypeIconUrl { get; init; }
}

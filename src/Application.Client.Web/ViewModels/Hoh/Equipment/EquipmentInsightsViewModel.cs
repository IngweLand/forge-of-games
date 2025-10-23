using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;

public class EquipmentInsightsViewModel
{
    public required string LevelRange { get; init; }
    public required EquipmentSlotType EquipmentSlotType { get; init; }
    public required IReadOnlyCollection<IconLabelItemViewModel> EquipmentSets { get; init; }
    public required IReadOnlyCollection<IconLabelItemViewModel> MainAttributes { get; init; }
    public IReadOnlyCollection<IconLabelItemViewModel> SubAttributesLevel4 { get; init; } = [];
    public IReadOnlyCollection<IconLabelItemViewModel> SubAttributesLevel8 { get; init; }  = [];
    public IReadOnlyCollection<IconLabelItemViewModel> SubAttributesLevel12 { get; init; }  = [];
    public IReadOnlyCollection<IconLabelItemViewModel> SubAttributesLevel16 { get; init; } = [];
}

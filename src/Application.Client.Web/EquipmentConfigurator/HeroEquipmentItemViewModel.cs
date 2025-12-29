using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class HeroEquipmentItemViewModel
{
    public required IReadOnlyCollection<IconLabelItemViewModel> Attributes { get; init; }
    public required EquipmentItemViewModel EquipmentItemViewModel { get; init; }
    public required IconLabelItemViewModel EquipmentSet { get; init; }
}

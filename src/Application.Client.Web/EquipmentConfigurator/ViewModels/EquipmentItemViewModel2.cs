using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;

public class EquipmentItemViewModel2 : IconLabelItemViewModel
{
    public required IconLabelItemViewModel EquipmentSet { get; init; }

    public required int Id { get; init; }
    public required EquipmentItemAttributeViewModel2 MainAttribute { get; init; }
    public required IReadOnlyCollection<EquipmentItemSubAttributeViewModel2> SubAttributes { get; init; }
}

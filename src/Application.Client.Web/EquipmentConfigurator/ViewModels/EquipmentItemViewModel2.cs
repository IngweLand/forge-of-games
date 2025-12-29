using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;

public class EquipmentItemViewModel2 : IconLabelItemViewModel
{
    public required EquipmentItem EquipmentItem { get; init; }
    public required IconLabelItemViewModel EquipmentSet { get; init; }

    public required int Id { get; init; }
    public required IconLabelsItemViewModel MainAttribute { get; init; }
    public required IReadOnlyCollection<EquipmentItemSubAttributeViewModel2> SubAttributes { get; init; }

    public ISet<HeroBasicViewModel> EquippedOnHeroes { get; set; } = new HashSet<HeroBasicViewModel>();
}

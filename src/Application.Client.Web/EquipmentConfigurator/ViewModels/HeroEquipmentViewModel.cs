using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;

public class HeroEquipmentViewModel
{
    public required HeroBasicViewModel Hero { get; init; }

    public IList<EquipmentConfigurationViewModel> Equipment { get; init; } =
        new List<EquipmentConfigurationViewModel>();
}

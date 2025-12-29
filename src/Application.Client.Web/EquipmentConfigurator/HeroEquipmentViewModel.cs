using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class HeroEquipmentViewModel
{
    public required HeroBasicViewModel Hero { get; init; }

    public IList<HeroEquipmentConfigurationViewModel> Equipment { get; init; } =
        new List<HeroEquipmentConfigurationViewModel>();
}

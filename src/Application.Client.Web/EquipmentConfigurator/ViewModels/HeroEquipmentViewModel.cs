using System.Collections.ObjectModel;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;

public class HeroEquipmentViewModel
{
    public ObservableCollection<EquipmentConfigurationViewModel> Equipment { get; init; } = [];

    public required HeroBasicViewModel Hero { get; init; }
}

using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Dtos.Hoh.City;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class CustomizationComponentViewModel
{
    public required IReadOnlyCollection<IconLabelItemViewModel> General { get; init; } = new List<IconLabelItemViewModel>();
    public required IReadOnlyCollection<BuildingCustomizationDto> Items { get; init; } = new List<BuildingCustomizationDto>();
    public required BuildingCustomizationDto SelectedItem { get; init; }
}

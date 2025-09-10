using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class ProductionComponentViewModel
{
    public bool CanSelectProduct { get; init; }
    public IReadOnlyCollection<IconLabelItemViewModel> General { get; init; } = new List<IconLabelItemViewModel>();

    public IReadOnlyCollection<CityMapEntityProductViewModel> Products { get; init; } =
        new List<CityMapEntityProductViewModel>();
    public IReadOnlyCollection<TimedProductionValuesViewModel> Cost { get; init; } =
        new List<TimedProductionValuesViewModel>();
}

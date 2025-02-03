namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Tools;

public class BuildingMultilevelCostViewModel
{
    public required IReadOnlyCollection<ConstructVsUpgradeItemViewModel> Costs { get; init; }
    public required int FromLevel { get; init; }
    public required int ToLevel { get; init; }
}

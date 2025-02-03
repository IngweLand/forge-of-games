namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class BuildingSelectorTypesViewModel
{
    public required IReadOnlyCollection<BuildingSelectorItemViewModel> BuildingGroups { get; init; } =
        new List<BuildingSelectorItemViewModel>();

    public required string? Icon { get; init; }
}

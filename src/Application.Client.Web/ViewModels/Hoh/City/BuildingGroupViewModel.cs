using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class BuildingGroupViewModel
{
    public required IReadOnlyCollection<BuildingViewModel> Buildings { get; init; } = new List<BuildingViewModel>();
    public required string BuildingSize { get; init; }
    public required string CityName { get; init; }
    public required BuildingGroup Id { get; init; }
    public string? ImageUrl { get; init; }
    public required string Name { get; init; }
    public required string TypeIconUrl { get; init; }
    public required string TypeName { get; init; }
}

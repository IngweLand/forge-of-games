using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class BuildingTypeViewModel
{
    public CityId CityId { get; set; }

    public required IReadOnlyCollection<BuildingGroupBasicViewModel> Groups { get; init; } =
        new List<BuildingGroupBasicViewModel>();

    public required string Name { get; init; }
}

using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class CityBuildingGroupsViewModel
{
    public required IReadOnlyCollection<BuildingTypeViewModel> BuildingTypes { get; init; } =
        new List<BuildingTypeViewModel>();

    public CityId CityId { get; set; }
    public required string CityName { get; init; }
}

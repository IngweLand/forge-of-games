using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityMapBuildingGroupViewModel
{
    public AgeViewModel? Age { get; init; }
    public required BuildingGroup BuildingGroup { get; init; }

    public int? Level { get; init; }

    public required IReadOnlyCollection<BuildingLevelSpecs> Levels { get; init; }
    public required string Name { get; init; }
}
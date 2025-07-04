using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;

public class AreaProvider:ICityMapEntityStats
{
    public required BuildingGroup BuildingGroup { get; init; }
    public required BuildingType BuildingType { get; init; }
    public required int Area { get; init; }
}

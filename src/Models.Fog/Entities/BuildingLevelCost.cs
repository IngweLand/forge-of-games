using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Models.Fog.Entities;

public class BuildingLevelCost
{
    public required IReadOnlyCollection<ResourceAmount> Cost { get; init; }
    public required int Time { get; init; }

    public static BuildingLevelCost Blank => new BuildingLevelCost() {Time = 0, Cost = []};
}

using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Models.Fog.Entities;

public class BuildingLevelCostData
{
    public required int Level { get; init; }
    public ConstructionComponent? ConstructionComponent { get; set; }
    public UpgradeComponent? UpgradeComponent { get; set; }
}

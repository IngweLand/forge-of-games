namespace Ingweland.Fog.Models.Fog.Entities;

public class BuildingMultilevelCost
{
    public required BuildingLevelCost ConstructionCost { get; init; }
    public required int CurrentLevel { get; init; }
    public required int ToLevel { get; init; }
    public required BuildingLevelCost UpgradeCost { get; init; }
}

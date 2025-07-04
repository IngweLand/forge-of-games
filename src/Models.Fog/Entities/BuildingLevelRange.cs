namespace Ingweland.Fog.Models.Fog.Entities;

public class BuildingLevelRange
{
    public required int StartLevel { get; init; }
    public required int EndLevel { get; init; }
    
    public static BuildingLevelRange Empty()
    {
        return new BuildingLevelRange() {StartLevel = 0, EndLevel = 0};
    }
}

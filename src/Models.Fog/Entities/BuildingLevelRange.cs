namespace Ingweland.Fog.Models.Fog.Entities;

public class BuildingLevelRange
{
    public static readonly BuildingLevelRange Empty = new() {StartLevel = 0, EndLevel = 0};
    public required int EndLevel { get; init; }
    public required int StartLevel { get; init; }
}

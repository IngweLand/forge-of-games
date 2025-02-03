namespace Ingweland.Fog.Models.Fog.Entities;

public class BasicHeroProfile
{
    public required string Id { get; init; }
    public required string HeroId { get; init; }
    public int Level { get; init; }
    public int AscensionLevel { get; init; }
    public int AbilityLevel { get; init; }
    public int AwakeningLevel { get; init; }
}

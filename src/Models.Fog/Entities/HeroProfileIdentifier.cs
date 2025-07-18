namespace Ingweland.Fog.Models.Fog.Entities;

public record HeroProfileIdentifier
{
    [Obsolete]
    public string? Id { get; init; }
    public required string HeroId { get; init; }
    public int Level { get; init; } = 1;
    public int AscensionLevel { get; init; }
    public int AbilityLevel { get; init; } = 1;
    public int AwakeningLevel { get; init; }
    
    public int BarracksLevel { get; init; }
}

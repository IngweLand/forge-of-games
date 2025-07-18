namespace Ingweland.Fog.Models.Fog.Entities;

public class CommandCenterProfileTeam
{
    [Obsolete]
    public HashSet<string> HeroProfileIds { get; init; } = [];
    
    public HashSet<string> HeroIds { get; init; } = [];
    public required string Id { get; init; }
    public required string Name { get; init; }
}

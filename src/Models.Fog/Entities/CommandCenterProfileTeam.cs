namespace Ingweland.Fog.Models.Fog.Entities;

public class CommandCenterProfileTeam
{
    public HashSet<string> HeroProfileIds { get; init; } = [];
    public required string Id { get; init; }
    public required string Name { get; init; }
}

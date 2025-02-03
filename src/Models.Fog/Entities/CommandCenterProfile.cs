namespace Ingweland.Fog.Models.Fog.Entities;

public class CommandCenterProfile
{
    public required BarracksProfile BarracksProfile { get; init; }
    public IDictionary<string, HeroProfile> Heroes { get; set; } = new Dictionary<string, HeroProfile>();
    public required string Id { get; init; }
    public required string Name { get; set; }

    public IDictionary<string, CommandCenterProfileTeam> Teams { get; init; } =
        new Dictionary<string, CommandCenterProfileTeam>();
}

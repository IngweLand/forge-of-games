namespace Ingweland.Fog.Models.Fog.Entities;

public class BasicCommandCenterProfile
{
    public required BarracksProfile BarracksProfile { get; init; }
    public IList<BasicHeroProfile> Heroes { get; init; } = new List<BasicHeroProfile>();
    public required string Id { get; set; }
    public required string Name { get; set; }
    public IList<CommandCenterProfileTeam> Teams { get; init; } = new List<CommandCenterProfileTeam>();

}

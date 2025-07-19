namespace Ingweland.Fog.Models.Fog.Entities;

public class BasicCommandCenterProfile : VersionedModel
{
    public required BarracksProfile BarracksProfile { get; init; }
    public IList<HeroProfileIdentifier> Heroes { get; init; } = new List<HeroProfileIdentifier>();
    public required string Id { get; set; }
    public required string Name { get; set; }
    public IList<CommandCenterProfileTeam> Teams { get; init; } = new List<CommandCenterProfileTeam>();

    public int CommandCenterVersion { get; set; }
}

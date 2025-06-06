namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

public class PvpUnitDetails
{
    public required string Id { get; init; }
    public required int Level { get; init; }
    public int AscensionLevel { get; init; }
    public int AbilityLevel { get; init; }
    public IReadOnlyCollection<string> Abilities { get; init; } = new List<string>();
}
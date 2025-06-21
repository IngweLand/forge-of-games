namespace Ingweland.Fog.Dtos.Hoh.Battle;

public class BattleSquadDto
{
    public IReadOnlyCollection<string> Abilities { get; set; } = new List<string>();

    public int AbilityLevel { get; set; }

    public int AscensionLevel { get; set; }

    public int Level { get; set; }

    public required string UnitId { get; set; }
}

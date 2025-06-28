namespace Ingweland.Fog.Dtos.Hoh.Battle;

public record BattleUnitDto
{
    public IReadOnlyCollection<string> Abilities { get; init; } = new List<string>();

    public int AbilityLevel { get; init; }

    public int AscensionLevel { get; init; }

    public int Level { get; init; }

    public required string UnitId { get; init; }

    public virtual bool Equals(BattleUnitDto? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return AbilityLevel == other.AbilityLevel &&
            AscensionLevel == other.AscensionLevel &&
            Level == other.Level &&
            UnitId == other.UnitId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AbilityLevel, AscensionLevel, Level, UnitId);
    }
}

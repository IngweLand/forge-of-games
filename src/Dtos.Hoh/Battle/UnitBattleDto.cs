using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

public record UnitBattleDto
{
    public required string BattleDefinitionId { get; init; }
    public UnitBattleStatsDto? BattleStats { get; init; }
    public required Difficulty Difficulty { get; init; }
    public required BattleUnitDto Unit { get; init; }
    public required string UnitName { get; init; }

    public virtual bool Equals(UnitBattleDto? other)

    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return BattleDefinitionId == other.BattleDefinitionId &&
            Difficulty == other.Difficulty &&
            Unit.Equals(other.Unit);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(BattleDefinitionId, Difficulty, Unit);
    }
}

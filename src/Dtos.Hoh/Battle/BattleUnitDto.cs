using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

public record BattleUnitDto : IBattleUnitProperties
{
    public required IReadOnlyDictionary<UnitStatType, float> FinalState { get; init; } =
        new Dictionary<UnitStatType, float>();

    public int? UnitInBattleId { get; init; }

    public IReadOnlyCollection<string> Abilities { get; init; } = new List<string>();

    public int AbilityLevel { get; init; }

    public int AscensionLevel { get; init; }

    public int Level { get; init; }

    public required IReadOnlyCollection<StatBoost> StatBoosts { get; init; } = new List<StatBoost>();

    public required string UnitId { get; init; }

    public IReadOnlyDictionary<UnitStatType, float> UnitStatsOverrides { get; init; } =
        new Dictionary<UnitStatType, float>();

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

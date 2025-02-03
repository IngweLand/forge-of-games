namespace Ingweland.Fog.Models.Fog.Entities;

public record HeroLevelSpecs : IComparable<HeroLevelSpecs>
{
    public required int AscensionLevel { get; init; }
    public bool IsAscension { get; init; }
    public required int Level { get; init; }
    public required string Title { get; init; }

    public int CompareTo(HeroLevelSpecs? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        var levelComparison = Level.CompareTo(other.Level);
        if (levelComparison != 0)
        {
            return levelComparison;
        }

        return AscensionLevel.CompareTo(other.AscensionLevel);
    }

    public static bool operator <(HeroLevelSpecs? left, HeroLevelSpecs? right)
    {
        if (left is null)
        {
            return right is not null;
        }

        return left.CompareTo(right) < 0;
    }

    public static bool operator >(HeroLevelSpecs? left, HeroLevelSpecs? right)
    {
        if (left is null)
        {
            return false;
        }

        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(HeroLevelSpecs? left, HeroLevelSpecs? right)
    {
        if (left is null)
        {
            return true;
        }

        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(HeroLevelSpecs? left, HeroLevelSpecs? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.CompareTo(right) >= 0;
    }
}

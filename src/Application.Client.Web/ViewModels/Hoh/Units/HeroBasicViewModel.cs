namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

public class HeroBasicViewModel : IEquatable<HeroBasicViewModel>
{
    public required string Id { get; init; }

    public required string Name { get; init; }
    public required string PortraitUrl { get; init; }
    public required int StarCount { get; init; }
    public required string UnitClassIconUrl { get; init; }

    public required string UnitColor { get; init; }
    public required string UnitId { get; init; }

    public required string UnitTypeIconUrl { get; init; }

    public bool Equals(HeroBasicViewModel? other)
        => other is not null && StringComparer.Ordinal.Equals(Id, other.Id);

    public override bool Equals(object? obj)
        => obj is HeroBasicViewModel other && Equals(other);

    public override int GetHashCode()
        => StringComparer.Ordinal.GetHashCode(Id);
}

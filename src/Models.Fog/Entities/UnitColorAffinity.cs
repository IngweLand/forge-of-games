using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class UnitColorAffinity
{
    public required UnitColor Color { get; init; }
    public required UnitColor StrongAgainst { get; init; }
    public required UnitColor WeakAgainst { get; init; }
}
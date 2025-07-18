using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class HeroProfile
{
    public required float AbilityChargeTime { get; init; }
    public required float AbilityInitialChargeTime { get; init; }

    public required HeroProfileIdentifier Identifier { get; init; }

    public required double Power { get; init; }

    public required IReadOnlyDictionary<UnitStatType, float> Stats { get; init; } =
        new Dictionary<UnitStatType, float>();

    public IReadOnlyDictionary<UnitStatType, IReadOnlyDictionary<UnitStatSource, float>> StatsBreakdown { get; init; } =
        new Dictionary<UnitStatType, IReadOnlyDictionary<UnitStatSource, float>>();

    public required HeroSupportUnitProfile SupportUnitProfile { get; init; }
}

using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class HeroProfile
{
    public required int AbilityLevel { get; set; }

    public required int AscensionLevel { get; set; }
    public required int AwakeningLevel { get; set; }
    public required string HeroId { get; init; }
    public required string Id { get; init; }
    public required int Level { get; set; }
    public required int BarracksLevel { get; set; }

    public required IReadOnlyDictionary<UnitStatType, float> Stats { get; set; } =
        new Dictionary<UnitStatType, float>();

    public required float AbilityChargeTime { get; init; }
    public required float AbilityInitialChargeTime { get; init; }

    public required double Power { get; set; }
    public required HeroSupportUnitProfile SupportUnitProfile { get; set; }
}

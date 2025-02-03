using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class HeroSupportUnitProfile
{
    public required IReadOnlyDictionary<UnitStatType, float> Stats { get; set; }
    public required double Power { get; init; }
    public required IUnit Unit { get; init; }
    public required string AssetIt { get; init; }
}

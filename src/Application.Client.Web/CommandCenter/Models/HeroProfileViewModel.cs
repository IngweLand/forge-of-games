using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Models;

public record HeroProfileViewModel
{
    public required string AbilityChargeTime { get; init; }
    public required string AbilityDescription { get; init; }

    public required string AbilityIconUrl { get; init; }
    public required float AbilityInitialChargePercentage { get; init; }
    public required string AbilityInitialChargeTime { get; init; }
    public required int AbilityLevel { get; init; }
    public required IReadOnlyCollection<int> AbilityLevels { get; init; }
    public required int AwakeningLevel { get; init; }
    public required IReadOnlyCollection<int> AwakeningLevels { get; init; }
    public required int BarracksLevel { get; init; }
    public IReadOnlyCollection<int>? BarracksLevels { get; init; }
    public required string HeroId { get; init; }
    public required IReadOnlyCollection<HeroLevelSpecs> HeroLevels { get; init; }
    public required string HeroUnitId { get; init; }
    public required string Id { get; init; }
    public required HeroLevelSpecs Level { get; init; }
    public required string Name { get; init; }
    public required string PortraitUrl { get; init; }
    public required int Power { get; init; }
    public int StarCount { get; init; }
    public IReadOnlyCollection<UnitStatBreakdownViewModel> StatsBreakdown { get; init; } = [];
    public required IReadOnlyCollection<IconLabelItemViewModel> StatsItems { get; init; }
    public required HeroSupportUnitViewModel? SupportUnit { get; init; }

    public int TotalPower
    {
        get
        {
            var supportUnitPower = SupportUnit?.Power ?? 0;
            return Power + supportUnitPower;
        }
    }

    public required string UnitClassIconUrl { get; init; }

    public required string UnitClassName { get; init; }
    public required string UnitClassTintedIconUrl { get; init; }
    public required string UnitColor { get; init; }
    public required string UnitTypeIconUrl { get; init; }
    public required string UnitTypeName { get; init; }
    public required string UnitTypeTintedIconUrl { get; init; }
}

using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Models;

public class HeroProfileViewModel
{
    public required string AbilityChargeTime { get; init; }
    public required string AbilityDescription { get; init; }

    public required string AbilityIconUrl { get; init; }
    public required float AbilityInitialChargePercentage { get; set; }
    public required string AbilityInitialChargeTime { get; init; }
    public required int AbilityLevel { get; init; }
    public required IReadOnlyCollection<int> AbilityLevels { get; set; }
    public required int AwakeningLevel { get; init; }
    public required IReadOnlyCollection<int> AwakeningLevels { get; set; }
    public required int BarracksLevel { get; set; }
    public IReadOnlyCollection<int>? BarracksLevels { get; init; }
    public required string HeroId { get; init; }
    public required IReadOnlyCollection<HeroLevelSpecs> HeroLevels { get; set; }
    public required string HeroUnitId { get; set; }
    public required string Id { get; init; }
    public required HeroLevelSpecs Level { get; init; }
    public required string Name { get; init; }
    public required string PortraitUrl { get; init; }
    public required int Power { get; init; }
    public int StarCount { get; init; }
    public IReadOnlyCollection<UnitStatBreakdownViewModel> StatsBreakdown { get; init; } = [];
    public required IReadOnlyCollection<IconLabelItemViewModel> StatsItems { get; set; }
    public required HeroSupportUnitViewModel? SupportUnit { get; init; }

    public int TotalPower
    {
        get
        {
            var supportUnitPower = SupportUnit?.Power ?? 0;
            return Power + supportUnitPower;
        }
    }

    public required string UnitClassIconUrl { get; set; }

    public required string UnitClassName { get; set; }
    public required string UnitClassTintedIconUrl { get; set; }
    public required string UnitColor { get; init; }
    public required string UnitTypeIconUrl { get; set; }
    public required string UnitTypeName { get; set; }
    public required string UnitTypeTintedIconUrl { get; set; }
}

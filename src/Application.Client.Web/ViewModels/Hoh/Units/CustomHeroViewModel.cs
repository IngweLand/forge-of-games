using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

public class CustomHeroViewModel
{
    public required string PortraitUrl { get; init; }
    public required IReadOnlyCollection<IconLabelItemViewModel> StatsItems { get; init; }
    public required string AbilityIconUrl { get; init; }
    public required HeroSupportUnitViewModel? SupportUnit { get; init; }
    public required string AbilityDescription { get; init; }
    public required IReadOnlyDictionary<UnitStatType, float> Stats { get; init; }
    public required int Power { get; init; }
    public int TotalPower
    {
        get
        {
            var supportUnitPower = SupportUnit?.Power ?? 0;
            return Power + supportUnitPower;
        }
    }
}

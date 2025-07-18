namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

public class HeroAbilityViewModel
{
    public required string ChargeTime { get; init; }
    public required float InitialChargePercentage { get; init; }
    public required string InitialChargeTime { get; init; }
    public required string IconUrl { get; init; }

    public required HeroAbilityLevelViewModel Level { get; init; }

    public required string Name { get; init; }
}

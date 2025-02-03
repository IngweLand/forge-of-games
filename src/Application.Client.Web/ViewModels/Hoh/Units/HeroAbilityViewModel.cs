namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

public class HeroAbilityViewModel
{
    public required string IconUrl { get; init; }

    public required IReadOnlyList<HeroAbilityLevelViewModel> Levels { get; init; } =
        new List<HeroAbilityLevelViewModel>();

    public required string Name { get; init; }
}

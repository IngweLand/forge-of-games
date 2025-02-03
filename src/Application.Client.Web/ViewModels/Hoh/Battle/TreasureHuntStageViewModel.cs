namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class TreasureHuntStageViewModel
{
    public required string DifficultyIconUrl { get; init; }
    public required string DifficultyName { get; init; }
    public required IReadOnlyCollection<TreasureHuntEncounterViewModel> Encounters { get; init; }
    public required string Name { get; init; }
}

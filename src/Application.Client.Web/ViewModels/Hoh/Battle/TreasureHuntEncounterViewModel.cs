namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class TreasureHuntEncounterViewModel
{
    public required string Title { get; init; }
    public required IReadOnlyCollection<BattleWaveViewModel> Waves { get; init; }
}

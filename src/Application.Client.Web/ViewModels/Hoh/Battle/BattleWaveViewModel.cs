namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class BattleWaveViewModel
{
    public required IReadOnlyCollection<BattleWaveSquadViewModel> Squads { get; init; }
    public required string Title { get; init; }
    public double Power => Math.Ceiling(Squads.Sum(src => src.Power));
}

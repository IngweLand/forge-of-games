namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class BattleStatsViewModel
{
    public static readonly BattleStatsViewModel Blank = new();

    public IReadOnlyCollection<BattleStatsSquadViewModel> EnemySquads { get; init; } = [];

    public IReadOnlyCollection<BattleStatsSquadViewModel> PlayerSquads { get; init; } = [];
}

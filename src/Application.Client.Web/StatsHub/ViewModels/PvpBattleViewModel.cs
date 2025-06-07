namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class PvpBattleViewModel
{
    public required PlayerViewModel Player { get; init; }
    public required PlayerViewModel Opponent { get; init; }
    public required bool IsVictory { get; init; }
    public required IReadOnlyCollection<PvpUnitViewModel> PlayerUnits { get; init; }
    public required IReadOnlyCollection<PvpUnitViewModel> OpponentUnits { get; init; }
}
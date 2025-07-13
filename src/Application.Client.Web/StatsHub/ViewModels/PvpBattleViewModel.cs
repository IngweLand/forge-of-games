using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class PvpBattleViewModel
{
    public required PlayerViewModel Player { get; init; }
    public required PlayerViewModel Opponent { get; init; }
    public required bool IsVictory { get; init; }
    public required IReadOnlyCollection<BattleSquadViewModel> PlayerUnits { get; init; }
    public required IReadOnlyCollection<BattleSquadViewModel> OpponentUnits { get; init; }
    public int? StatsId { get; init; }
}
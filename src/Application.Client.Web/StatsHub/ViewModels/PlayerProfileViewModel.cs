using Ingweland.Fog.Dtos.Hoh.Stats;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class PlayerProfileViewModel
{
    public IReadOnlyCollection<StatsTimedStringValue> Ages { get; init; } = Array.Empty<StatsTimedStringValue>();
    public IReadOnlyCollection<string> Alliances { get; init; } = Array.Empty<string>();
    public string? Names { get; init; }
    public required PlayerViewModel Player { get; init; }
    public IReadOnlyCollection<PvpBattleViewModel> PvpBattles { get; init; } = Array.Empty<PvpBattleViewModel>();
    public IReadOnlyCollection<StatsTimedIntValue> PvpRankingPoints { get; init; } = Array.Empty<StatsTimedIntValue>();
    public IReadOnlyCollection<StatsTimedIntValue> RankingPoints { get; init; } = Array.Empty<StatsTimedIntValue>();
}
using Ingweland.Fog.Dtos.Hoh.Stats;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class PlayerWithRankingsViewModel
{
    public IReadOnlyCollection<StatsTimedStringValue> Ages { get; init; } = Array.Empty<StatsTimedStringValue>();
    public IReadOnlyCollection<string> Alliances { get; init; } = Array.Empty<string>();
    public IReadOnlyCollection<string> Names { get; init; } = Array.Empty<string>();
    public required PlayerViewModel Player { get; init; }
    public IReadOnlyCollection<StatsTimedIntValue> RankingPoints { get; init; } = Array.Empty<StatsTimedIntValue>();
    public IReadOnlyCollection<StatsTimedIntValue> PvpRankingPoints { get; init; } = Array.Empty<StatsTimedIntValue>();
}

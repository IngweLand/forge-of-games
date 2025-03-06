using Ingweland.Fog.Dtos.Hoh.Stats;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class AllianceWithRankingsViewModel
{
    public IReadOnlyCollection<StatsTimedStringValue> Names { get; init; } = Array.Empty<StatsTimedStringValue>();
    public required AllianceViewModel Alliance { get; init; }
    public IReadOnlyCollection<StatsTimedIntValue> RankingPoints { get; init; } = Array.Empty<StatsTimedIntValue>();
    public IReadOnlyCollection<PlayerViewModel> CurrentMembers { get; init; } = Array.Empty<PlayerViewModel>();
    public IReadOnlyCollection<PlayerViewModel> PossibleMembers { get; init; } = Array.Empty<PlayerViewModel>();
    public string? RegisteredAt { get; set; }
    public PlayerDto? Leader { get; set; }
}
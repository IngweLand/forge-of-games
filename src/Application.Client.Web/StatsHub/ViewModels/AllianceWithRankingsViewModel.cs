using Ingweland.Fog.Dtos.Hoh.Stats;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class AllianceWithRankingsViewModel
{
    public required AllianceViewModel Alliance { get; init; }

    public IReadOnlyCollection<AllianceMemberViewModel> CurrentMembers { get; init; } =
        Array.Empty<AllianceMemberViewModel>();

    public IReadOnlyCollection<StatsTimedStringValue> Names { get; init; } = Array.Empty<StatsTimedStringValue>();
    public IReadOnlyCollection<StatsTimedIntValue> RankingPoints { get; init; } = Array.Empty<StatsTimedIntValue>();
    public string? RegisteredAt { get; set; }
}

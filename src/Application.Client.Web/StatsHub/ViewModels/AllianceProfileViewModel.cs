using Ingweland.Fog.Dtos.Hoh.Stats;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class AllianceProfileViewModel
{
    public required AllianceViewModel Alliance { get; init; }

    public IReadOnlyCollection<AllianceMemberViewModel> CurrentMembers { get; init; } =
        Array.Empty<AllianceMemberViewModel>();

    public IReadOnlyCollection<StatsTimedStringValue> Names { get; init; } = Array.Empty<StatsTimedStringValue>();
}

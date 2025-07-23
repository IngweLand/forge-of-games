using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class TopStatsViewModel
{
    public required IReadOnlyCollection<AllianceViewModel> BetaWorldAlliances { get; init; }
    public required IReadOnlyCollection<PlayerViewModel> BetaWorldPlayers { get; init; }
    public required IReadOnlyCollection<HeroBasicViewModel> Heroes { get; init; }
    public required IReadOnlyCollection<AllianceViewModel> MainWorldAlliances { get; init; }
    public required IReadOnlyCollection<PlayerViewModel> MainWorldPlayers { get; init; }
}

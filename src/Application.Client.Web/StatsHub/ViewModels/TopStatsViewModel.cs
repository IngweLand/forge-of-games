namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class TopStatsViewModel
{
    public required IReadOnlyCollection<PlayerViewModel> MainWorldPlayers { get; init; }
    public required IReadOnlyCollection<PlayerViewModel> BetaWorldPlayers { get; init; }
}

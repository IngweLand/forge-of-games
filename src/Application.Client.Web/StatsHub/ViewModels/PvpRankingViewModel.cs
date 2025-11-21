using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class PvpRankingViewModel
{
    public required DateTime CollectedAt { get; set; }
    public required PvpTier Tier { get; set; }
}

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class AllianceViewModel
{
    public required AllianceBannerViewModel Banner { get; init; }
    public required int Id { get; init; }
    public bool IsDeleted { get; init; }
    public required bool IsStale { get; set; }
    public required string Name { get; set; }
    public required int Rank { get; set; }
    public required int RankingPoints { get; set; }
    public required string RankingPointsFormatted { get; set; }
    public required string UpdatedAt { get; set; }
    public required string WorldId { get; init; }
}

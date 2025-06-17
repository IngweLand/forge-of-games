namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class PlayerViewModel
{
    public required string Age { get; set; }
    public required string AgeColor { get; init; }
    public string? AllianceName { get; init; }
    public required string AvatarUrl { get; set; }
    public required int Id { get; init; }
    public required bool IsStale { get; set; }
    public required string Name { get; set; }
    public required string Rank { get; set; }
    public required string RankingPoints { get; set; }
    public required string RankingPointsFormatted { get; set; }
    public required string UpdatedAt { get; set; }
    public required string WorldId { get; init; }
}

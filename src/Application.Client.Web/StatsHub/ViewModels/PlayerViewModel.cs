using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class PlayerViewModel
{
    public required string Age { get; set; }
    public required string AgeColor { get; init; }
    public string? AllianceName { get; init; }
    public required string AvatarUrl { get; set; }
    public required bool IsStale { get; set; }
    public required PlayerKey Key { get; init; }
    public required string Name { get; set; }
    public required int Rank { get; set; }
    public required int RankingPoints { get; set; }
    public required string UpdatedAt { get; set; }
}

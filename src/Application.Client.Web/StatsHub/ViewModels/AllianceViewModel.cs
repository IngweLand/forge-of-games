using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class AllianceViewModel
{
    public required string AvatarIconUrl { get; set; }
    public required string AvatarBackgroundUrl { get; set; }
    public required bool IsStale { get; set; }
    public required AllianceKey Key { get; init; }
    public required string Name { get; set; }
    public required int Rank { get; set; }
    public required int RankingPoints { get; set; }
    public required string UpdatedAt { get; set; }
}
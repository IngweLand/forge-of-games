using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class AllianceMemberViewModel
{
    public required string Age { get; set; }
    public required string AgeColor { get; init; }
    public required string AvatarUrl { get; set; }
    public required bool IsStale { get; set; }
    public string? JoinedOn { get; set; }
    public required string Name { get; set; }
    public required int PlayerId { get; init; }
    public required string Rank { get; set; }
    public required string RankingPointsFormatted { get; set; }

    public string? RoleIconUrl { get; init; }
    public TreasureHuntDifficultyBasicViewModel? TreasureHuntDifficulty { get; init; }
    public int TreasureHuntMaxPoints { get; init; }
}

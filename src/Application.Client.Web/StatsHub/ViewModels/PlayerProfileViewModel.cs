using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Stats;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class PlayerProfileViewModel
{
    public IReadOnlyCollection<StatsTimedStringValue> Ages { get; init; } = [];
    public required IReadOnlyCollection<DateTime> CitySnapshotDays { get; init; } = [];
    public AllianceViewModel? CurrentAlliance { get; init; }
    public bool HasPvpBattles { get; init; }
    public string? Names { get; init; }
    public required PlayerViewModel Player { get; init; }
    public IReadOnlyCollection<AllianceViewModel> PreviousAlliances { get; init; } = [];
    public IReadOnlyCollection<StatsTimedIntValue> PvpRankingPoints { get; init; } = [];
    public IReadOnlyCollection<StatsTimedIntValue> RankingPoints { get; init; } = [];
    public IReadOnlyCollection<HeroProfileBasicViewModel> Squads { get; init; } = [];
    public TreasureHuntDifficultyBasicViewModel? TreasureHuntDifficulty { get; init; }
    public int TreasureHuntMaxPoints { get; init; }
}

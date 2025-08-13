using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.Stats;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class PlayerProfileViewModel
{
    public IReadOnlyCollection<StatsTimedStringValue> Ages { get; init; } = [];
    public IReadOnlyCollection<AllianceViewModel> PreviousAlliances { get; init; } = [];
    public AllianceViewModel? CurrentAlliance { get; init; } 
    public string? Names { get; init; }
    public required PlayerViewModel Player { get; init; }
    public IReadOnlyCollection<HeroProfileViewModel> Squads { get; init; } = [];
    public IReadOnlyCollection<PvpBattleViewModel> PvpBattles { get; init; } = [];
    public IReadOnlyCollection<StatsTimedIntValue> PvpRankingPoints { get; init; } = [];
    public IReadOnlyCollection<StatsTimedIntValue> RankingPoints { get; init; } = [];
    public TreasureHuntDifficultyBasicViewModel? TreasureHuntDifficulty { get; init; }
    public int TreasureHuntMaxPoints { get; init; }
}

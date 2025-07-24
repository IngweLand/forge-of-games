using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class PlayerProfile
{
    public IReadOnlyCollection<StatsTimedStringValue> Ages { get; init; } = [];
    public IReadOnlyCollection<string> Alliances { get; init; } = [];
    public IReadOnlyCollection<string> Names { get; init; } = [];
    public required PlayerDto Player { get; init; }
    public IReadOnlyCollection<PvpBattleDto> PvpBattles { get; init; } = [];
    public IReadOnlyCollection<StatsTimedIntValue> PvpRankingPoints { get; init; } = [];
    public IReadOnlyCollection<StatsTimedIntValue> RankingPoints { get; init; } = [];
    public int? TreasureHuntDifficulty { get; set; }
}
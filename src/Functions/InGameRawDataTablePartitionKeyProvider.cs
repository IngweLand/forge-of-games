using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Functions;

public class InGameRawDataTablePartitionKeyProvider
{
    public string AllianceRankings(string worldId, DateOnly date, AllianceRankingType rankingType)
    {
        return WithWorldAndDate("alliance-rankings", worldId, date) + $"_{rankingType}";
    }

    public string PlayerRankings(string worldId, DateOnly date, PlayerRankingType rankingType)
    {
        return WithWorldAndDate("player-rankings", worldId, date) + $"_{rankingType}";
    }

    public string PvpRankings(string worldId, DateOnly date)
    {
        return WithWorldAndDate("pvp-rankings", worldId, date);
    }

    public string PvpBattles(string worldId, DateOnly date)
    {
        return WithWorldAndDate("pvp-battles", worldId, date);
    }

    public string BattleStats(string worldId, DateOnly date)
    {
        return WithWorldAndDate("battle-stats", worldId, date);
    }

    public string BattleCompleteWave(string worldId, DateOnly date)
    {
        return WithWorldAndDate("battle-complete-wave", worldId, date);
    }

    public string BattleStart(string worldId, DateOnly date)
    {
        return WithWorldAndDate("battle-start", worldId, date);
    }

    public string AthAllianceRankings(string worldId, DateOnly date)
    {
        return WithWorldAndDate("ath-alliance-rankings", worldId, date);
    }

    public string Alliance(string worldId, DateOnly date)
    {
        return WithWorldAndDate("alliance", worldId, date);
    }

    private string WithWorldAndDate(string src, string worldId, DateOnly date)
    {
        return $"{src}_{worldId}_{date.ToString("O")}";
    }
}

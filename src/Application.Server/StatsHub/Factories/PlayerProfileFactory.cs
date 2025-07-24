using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using PvpBattle = Ingweland.Fog.Models.Fog.Entities.PvpBattle;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class PlayerProfileFactory(IMapper mapper, IPlayerBattlesFactory playerBattlesFactory)
    : IPlayerProfileFactory
{
    public PlayerProfile Create(Player player, IReadOnlyCollection<PvpBattle> pvpBattles,
        IReadOnlyDictionary<byte[], int> existingStatsIds)
    {
        var alliances = player.AllianceHistory.Select(a => a.Name)
            .Concat(player.AllianceNameHistory.Select(n => n.AllianceName))
            .ToHashSet()
            .Order()
            .ToList();

        var battles = pvpBattles.Select(x =>
        {
            int? statsId = null;
            if (existingStatsIds.TryGetValue(x.InGameBattleId, out var value))
            {
                statsId = value;
            }

            return playerBattlesFactory.Create(x, statsId);
        }).ToList();
        
        return new PlayerProfile
        {
            Player = mapper.Map<PlayerDto>(player),
            RankingPoints = CreateTimedIntValueCollection(player.Rankings, PlayerRankingType.PowerPoints),
            PvpRankingPoints = CreateTimedIntValueCollection(player.PvpRankings),
            Ages = CreateTimedStringValueCollection(player.AgeHistory, entry => entry.Age),
            Alliances = alliances,
            Names = player.NameHistory.Select(entry => entry.Name).ToList(),
            PvpBattles = battles,
            TreasureHuntDifficulty = player.TreasureHuntDifficulty,
        };
    }

    private static List<StatsTimedIntValue> CreateTimedIntValueCollection(
        IEnumerable<PlayerRanking> rankings,
        PlayerRankingType playerRankingType)
    {
        return rankings
            .Where(pr => pr.Type == playerRankingType)
            .OrderBy(pr => pr.CollectedAt)
            .Select(pr => new StatsTimedIntValue
                {Value = pr.Points, Date = pr.CollectedAt.ToDateTime(TimeOnly.MinValue)})
            .ToList();
    }

    private static List<StatsTimedIntValue> CreateTimedIntValueCollection(IEnumerable<PvpRanking> rankings)
    {
        return rankings
            .OrderBy(pr => pr.CollectedAt)
            .Select(pr => new StatsTimedIntValue {Value = pr.Points, Date = pr.CollectedAt})
            .ToList();
    }

    private static List<StatsTimedStringValue> CreateTimedStringValueCollection<THistoryEntry>(
        IEnumerable<THistoryEntry> items,
        Func<THistoryEntry, string?> valueSelector) where THistoryEntry : IHistoryEntry
    {
        return items
            .OrderBy(entry => entry.ChangedAt)
            .Select(entry => new StatsTimedStringValue
                {Value = valueSelector(entry) ?? string.Empty, Date = DateOnly.FromDateTime(entry.ChangedAt)})
            .ToList();
    }
}

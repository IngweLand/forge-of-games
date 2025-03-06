using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class PlayerWithRankingsFactory(IMapper mapper) : IPlayerWithRankingsFactory
{
    public PlayerWithRankings Create(Player player)
    {
        var alliances = player.AllianceHistory.Select(a => a.Name)
            .Concat(player.AllianceNameHistory.Select(n => n.AllianceName))
            .ToHashSet()
            .Order()
            .ToList();
        
        return new PlayerWithRankings()
        {
            Player = mapper.Map<PlayerDto>(player),
            RankingPoints = CreateTimedIntValueCollection(player.Rankings, PlayerRankingType.RankingPoints),
            PvpRankingPoints = CreateTimedIntValueCollection(player.PvpRankings),
            Ages = CreateTimedStringValueCollection(player.AgeHistory, entry => entry.Age),
            Alliances = alliances,
            Names = player.NameHistory.Select(entry => entry.Name).ToList()
        };
    }

    private static List<StatsTimedIntValue> CreateTimedIntValueCollection(
        IEnumerable<PlayerRanking> rankings,
        PlayerRankingType playerRankingType)
    {
        return rankings
            .Where(pr => pr.Type == playerRankingType)
            .OrderBy(pr => pr.CollectedAt)
            .Select(pr => new StatsTimedIntValue() {Value = pr.Points, Date = pr.CollectedAt.ToDateTime(TimeOnly.MinValue)})
            .ToList();
    }
    
    private static List<StatsTimedIntValue> CreateTimedIntValueCollection(IEnumerable<PvpRanking> rankings)
    {
        return rankings
            .OrderBy(pr => pr.CollectedAt)
            .Select(pr => new StatsTimedIntValue() {Value = pr.Points, Date = pr.CollectedAt})
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
    
    private class TempHistoryEntry : IHistoryEntry
    {
        public DateTime ChangedAt { get; set; }
        public string? Value { get; init; }
    }
}
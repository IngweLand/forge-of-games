using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;
using PvpBattle = Ingweland.Fog.Models.Fog.Entities.PvpBattle;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class PlayerWithRankingsFactory(IMapper mapper, IUnitService unitService) : IPlayerWithRankingsFactory
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonStringEnumConverter()},
    };
    public async Task<PlayerWithRankings?> CreateAsync(Player player, IReadOnlyCollection<PvpBattle> pvpBattles,
        IReadOnlyDictionary<byte[], int> existingStatsIds)
    {
        var alliances = player.AllianceHistory.Select(a => a.Name)
            .Concat(player.AllianceNameHistory.Select(n => n.AllianceName))
            .ToHashSet()
            .Order()
            .ToList();

        var pvpBattleDtos = pvpBattles
            .Select(b =>
            {
                int? statsId = null;
                if (existingStatsIds.TryGetValue(b.InGameBattleId, out var value))
                {
                    statsId = value;
                }
                var winnerSquads = JsonSerializer.Deserialize<IReadOnlyCollection<BattleSquad>>(b.WinnerUnits,
                    JsonSerializerOptions) ?? [];
                var loserSquads = JsonSerializer.Deserialize<IReadOnlyCollection<BattleSquad>>(b.LoserUnits,
                    JsonSerializerOptions) ?? [];
                return new PvpBattleDto
                {
                    Winner = mapper.Map<PlayerDto>(b.Winner),
                    Loser = mapper.Map<PlayerDto>(b.Loser),
                    WinnerUnits = mapper.Map<IReadOnlyCollection<BattleSquadDto>>(winnerSquads),
                    LoserUnits = mapper.Map<IReadOnlyCollection<BattleSquadDto>>(loserSquads),
                    StatsId = statsId,
                };
            })
            .ToList();
        var heroIds = pvpBattleDtos.SelectMany(b => b.WinnerUnits.Select(u => u.Hero!.UnitId))
            .Concat(pvpBattleDtos.SelectMany(b => b.LoserUnits.Select(u => u.Hero!.UnitId)))
            .ToHashSet();
        var heroTasks = heroIds.Select(unitService.GetHeroAsync);
        var heroes = await Task.WhenAll(heroTasks);

        return new PlayerWithRankings
        {
            Player = mapper.Map<PlayerDto>(player),
            RankingPoints = CreateTimedIntValueCollection(player.Rankings, PlayerRankingType.PowerPoints),
            PvpRankingPoints = CreateTimedIntValueCollection(player.PvpRankings),
            Ages = CreateTimedStringValueCollection(player.AgeHistory, entry => entry.Age),
            Alliances = alliances,
            Names = player.NameHistory.Select(entry => entry.Name).ToList(),
            PvpBattles = pvpBattleDtos,
            Heroes = heroes!,
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

    private class TempHistoryEntry : IHistoryEntry
    {
        public string? Value { get; init; }
        public DateTime ChangedAt { get; set; }
    }
}

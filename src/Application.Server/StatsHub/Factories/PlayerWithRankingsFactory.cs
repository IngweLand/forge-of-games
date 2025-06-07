using System.Text.Json;
using AutoMapper;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class PlayerWithRankingsFactory(IMapper mapper, IUnitService unitService) : IPlayerWithRankingsFactory
{
    public async Task<PlayerWithRankings> CreateAsync(Player player)
    {
        var alliances = player.AllianceHistory.Select(a => a.Name)
            .Concat(player.AllianceNameHistory.Select(n => n.AllianceName))
            .ToHashSet()
            .Order()
            .ToList();

        var pvpBattles = player.PvpWins.Concat(player.PvpLosses)
            .OrderByDescending(b => b.PerformedAt)
            .Take(FogConstants.MaxDisplayedPvpBattles)
            .Select(b => new PvpBattleDto()
            {
                Winner = mapper.Map<PlayerDto>(b.Winner),
                Loser = mapper.Map<PlayerDto>(b.Loser),
                WinnerUnits = JsonSerializer.Deserialize<IReadOnlyCollection<PvpUnit>>(b.WinnerUnits)!,
                LoserUnits = JsonSerializer.Deserialize<IReadOnlyCollection<PvpUnit>>(b.LoserUnits)!
            })
            .ToList();
        var heroIds = pvpBattles.SelectMany(b => b.WinnerUnits.Select(u => u.Hero.Id))
            .Concat(pvpBattles.SelectMany(b => b.LoserUnits.Select(u => u.Hero.Id)))
            .ToHashSet();
        var heroTasks = heroIds.Select(unitService.GetHeroAsync);
        var heroes = await Task.WhenAll(heroTasks);

        return new PlayerWithRankings()
        {
            Player = mapper.Map<PlayerDto>(player),
            RankingPoints = CreateTimedIntValueCollection(player.Rankings, PlayerRankingType.RankingPoints),
            PvpRankingPoints = CreateTimedIntValueCollection(player.PvpRankings),
            Ages = CreateTimedStringValueCollection(player.AgeHistory, entry => entry.Age),
            Alliances = alliances,
            Names = player.NameHistory.Select(entry => entry.Name).ToList(),
            PvpBattles = pvpBattles,
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
            .Select(pr => new StatsTimedIntValue()
                {Value = pr.Points, Date = pr.CollectedAt.ToDateTime(TimeOnly.MinValue)})
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
        public string? Value { get; init; }
        public DateTime ChangedAt { get; set; }
    }
}
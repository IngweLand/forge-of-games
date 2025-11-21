using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class PlayerProfileDtoFactory(IMapper mapper) : IPlayerProfileDtoFactory
{
    public PlayerProfileDto Create(Player player, bool hasPvpBattles, IReadOnlyCollection<DateOnly> citySnapshotDays)
    {
        var uniqueAlliances = player.AllianceHistory
            .DistinctBy(x => x.Id)
            .OrderBy(x => x.Name)
            .ToList();

        return new PlayerProfileDto
        {
            Player = mapper.Map<PlayerDto>(player),
            RankingPoints = CreateTimedIntValueCollection(player.Rankings, PlayerRankingType.TotalHeroPower),
            Ages = CreateTimedStringValueCollection(player.AgeHistory, entry => entry.Age),
            Alliances = mapper.Map<IReadOnlyCollection<AllianceDto>>(uniqueAlliances),
            Names = player.NameHistory.Select(entry => entry.Name).ToList(),
            TreasureHuntDifficulty = player.TreasureHuntDifficulty,
            Squads = mapper.Map<IReadOnlyCollection<ProfileSquadDto>>(player.Squads),
            CitySnapshotDays = citySnapshotDays,
            HasPvpBattles = hasPvpBattles,
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

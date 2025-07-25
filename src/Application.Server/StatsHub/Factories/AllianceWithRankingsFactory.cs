using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class AllianceWithRankingsFactory(IMapper mapper) : IAllianceWithRankingsFactory
{
    public AllianceWithRankings Create(Alliance alliance, IReadOnlyCollection<PlayerDto> currentMembers)
    {
        return new AllianceWithRankings()
        {
            Alliance = mapper.Map<AllianceDto>(alliance),
            RankingPoints = CreateTimedIntValueCollection(alliance.Rankings, AllianceRankingType.TotalPoints),
            Names = CreateTimedStringValueCollection(alliance.NameHistory, entry => entry.Name),
            CurrentMembers = currentMembers,
            RegisteredAt = alliance.RegisteredAt?.ToDateOnly(),
            Leader = alliance.Leader != null ? mapper.Map<PlayerDto>(alliance.Leader) : null
        };
    }

    private static List<StatsTimedIntValue> CreateTimedIntValueCollection(
        IEnumerable<AllianceRanking> rankings,
        AllianceRankingType allianceRankingType)
    {
        return rankings.Where(pr => pr.Type == allianceRankingType)
            .OrderBy(pr => pr.CollectedAt).Select(pr => new StatsTimedIntValue()
                {Value = pr.Points, Date = pr.CollectedAt.ToDateTime(TimeOnly.MinValue)}).ToList();
    }

    private static List<StatsTimedStringValue> CreateTimedStringValueCollection<THistoryEntry>(
        IEnumerable<THistoryEntry> items,
        Func<THistoryEntry, string> valueSelector) where THistoryEntry : IHistoryEntry
    {
        return items
            .OrderBy(entry => entry.ChangedAt)
            .Select(entry => new StatsTimedStringValue
                {Value = valueSelector(entry), Date = DateOnly.FromDateTime(entry.ChangedAt)})
            .ToList();
    }
}
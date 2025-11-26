using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class AllianceWithRankingsFactory(IMapper mapper) : IAllianceWithRankingsFactory
{
    public AllianceWithRankings Create(Alliance alliance)
    {
        return new AllianceWithRankings
        {
            Alliance = mapper.Map<AllianceDto>(alliance),
            RankingPoints = CreateTimedIntValueCollection(alliance.Rankings, AllianceRankingType.MemberTotal),
            Names = CreateTimedStringValueCollection(alliance.NameHistory, entry => entry.Name),
            CurrentMembers = alliance.Members.OrderByDescending(x => x.Player.RankingPoints)
                .Select((entity, i) => CreateMember(entity, i + 1)).ToList(),
        };
    }

    private AllianceMemberDto CreateMember(AllianceMemberEntity member, int rank)
    {
        return new AllianceMemberDto
        {
            Name = member.Player.Name,
            Age = member.Player.Age,
            PlayerId = member.Player.Id,
            UpdatedAt = member.Player.UpdatedAt,
            JoinedAt = member.JoinedAt,
            Role = member.Role,
            AvatarId = member.Player.AvatarId,
            RankingPoints = member.Player.RankingPoints ?? 0,
            Rank = rank,
        };
    }

    private static List<StatsTimedIntValue> CreateTimedIntValueCollection(
        IEnumerable<AllianceRanking> rankings,
        AllianceRankingType allianceRankingType)
    {
        return rankings.Where(pr => pr.Type == allianceRankingType)
            .OrderBy(pr => pr.CollectedAt).Select(pr => new StatsTimedIntValue
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

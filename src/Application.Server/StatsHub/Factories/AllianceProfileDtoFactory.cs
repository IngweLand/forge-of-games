using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class AllianceProfileDtoFactory(IMapper mapper) : IAllianceProfileDtoFactory
{
    public AllianceProfileDto Create(Alliance alliance)
    {
        return new AllianceProfileDto
        {
            Alliance = mapper.Map<AllianceDto>(alliance),
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
            LastSeenAt = member.Player.LastSeenOnline.StripToHour(),
            Role = member.Role,
            AvatarId = member.Player.AvatarId,
            RankingPoints = member.Player.RankingPoints ?? 0,
            Rank = rank,
            TreasureHuntDifficulty = member.Player.TreasureHuntDifficulty ?? 0,
        };
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

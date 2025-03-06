using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class AllianceWithRankingsFactory(IMapper mapper) : IAllianceWithRankingsFactory
{
    public AllianceWithRankings Create(Alliance alliance, IReadOnlyCollection<PlayerDto> currentMembers,
        IReadOnlyCollection<PlayerDto> possiblePastMembers)
    {
        return new AllianceWithRankings()
        {
            Alliance = mapper.Map<AllianceDto>(alliance),
            RankingPoints = CreateTimedIntValueCollection(alliance.Rankings, AllianceRankingType.RankingPoints),
            Names = CreateTimedStringValueCollection(alliance.Rankings, pr => pr.Name),
            CurrentMembers = currentMembers,
            PossiblePastMembers = possiblePastMembers,
        };
    }

    private static List<StatsTimedIntValue> CreateTimedIntValueCollection(
        IEnumerable<AllianceRanking> rankings,
        AllianceRankingType allianceRankingType)
    {
        return rankings.Where(pr => pr.Type == allianceRankingType)
            .OrderBy(pr => pr.CollectedAt).Select(pr => new StatsTimedIntValue()
                {Value = pr.Points, Date = pr.CollectedAt}).ToList();
    }

    private static List<StatsTimedStringValue> CreateTimedStringValueCollection(
        IEnumerable<AllianceRanking> rankings, Func<AllianceRanking, string?> selector)
    {
        return rankings
            .Where(pr => selector.Invoke(pr) != null)
            .OrderBy(pr => pr.CollectedAt)
            .Aggregate(new List<StatsTimedStringValue>(), (acc, pr) =>
            {
                var currentValue = selector.Invoke(pr);
                if (acc.Count == 0 || acc.Last().Value != currentValue)
                {
                    acc.Add(new StatsTimedStringValue {Value = currentValue!, Date = pr.CollectedAt});
                }

                return acc;
            });
    }
}
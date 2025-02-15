using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class PlayerWithRankingsFactory(IMapper mapper) : IPlayerWithRankingsFactory
{
    public PlayerWithRankings Create(Player player)
    {
        return new PlayerWithRankings()
        {
            Player = mapper.Map<PlayerDto>(player),
            RankingPoints = CreateTimedIntValueCollection(player.Rankings, PlayerRankingType.RankingPoints),
            ResearchPoints = CreateTimedIntValueCollection(player.Rankings, PlayerRankingType.ResearchPoints),
            Ages = CreateTimedStringValueCollection(player.Rankings, pr => pr.Age),
            Alliances = CreateTimedStringValueCollection(player.Rankings, pr => pr.AllianceName),
            Names = CreateTimedStringValueCollection(player.Rankings, pr => pr.Name),
        };
    }

    private static IReadOnlyCollection<StatsTimedIntValue> CreateTimedIntValueCollection(
        IEnumerable<PlayerRanking> rankings,
        PlayerRankingType playerRankingType)
    {
        return rankings.Where(pr => pr.Type == playerRankingType)
            .OrderBy(pr => pr.CollectedAt).Select(pr => new StatsTimedIntValue()
                {Value = pr.Points, Date = pr.CollectedAt}).ToList();
    }

    private static IReadOnlyCollection<StatsTimedStringValue> CreateTimedStringValueCollection(
        IEnumerable<PlayerRanking> rankings, Func<PlayerRanking, string?> selector)
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

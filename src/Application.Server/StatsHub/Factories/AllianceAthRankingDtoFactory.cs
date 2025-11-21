using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class AllianceAthRankingDtoFactory : IAllianceAthRankingDtoFactory
{
    public AllianceAthRankingDto Create(AllianceAthRanking entity, InGameEventEntity inGameEvent)
    {
        return new AllianceAthRankingDto
        {
            Points = entity.Points,
            League = Enum.IsDefined(typeof(TreasureHuntLeague), entity.League)
                ? (TreasureHuntLeague)entity.League
                : TreasureHuntLeague.Undefined,
            StartedAt = inGameEvent.StartAt,
            EndedAt = inGameEvent.EndAt,
        };
    }
}

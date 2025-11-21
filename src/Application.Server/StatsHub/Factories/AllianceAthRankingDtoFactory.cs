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
            League = entity.League,
            StartedAt = inGameEvent.StartAt,
            EndedAt = inGameEvent.EndAt,
        };
    }
}

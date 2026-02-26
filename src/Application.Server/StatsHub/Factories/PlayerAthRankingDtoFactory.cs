using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class PlayerAthRankingDtoFactory : IPlayerAthRankingDtoFactory
{
    public PlayerAthRankingDto Create(PlayerAthRanking entity, InGameEventEntity inGameEvent)
    {
        return new PlayerAthRankingDto
        {
            Points = entity.Points,
            StartedAt = inGameEvent.StartAt,
            EndedAt = inGameEvent.EndAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}

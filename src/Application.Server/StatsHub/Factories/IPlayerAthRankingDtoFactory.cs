using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public interface IPlayerAthRankingDtoFactory
{
    PlayerAthRankingDto Create(PlayerAthRanking entity, InGameEventEntity inGameEvent);
}

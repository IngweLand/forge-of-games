using FluentResults;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Services.Interfaces;

public interface IFogRankingService
{
    Task<Result> UpsertRankingsAsync(string worldId, DateOnly collectedAt, IEnumerable<PlayerRank> rankings,
        PlayerRankingType rankingType);
}

using FluentResults;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;

namespace Ingweland.Fog.Application.Server.Services;

public class RankingUpdateOrchestrator(
    IInnSdkClient innSdkClient,
    IFogRankingService rankingService,
    IFogPlayerService playerService) : IRankingUpdateOrchestrator
{
    public async Task<Result> FetchAndStoreRankingAsync(GameWorldConfig gameWorld, PlayerRankingType rankingType)
    {
        var now = DateTime.UtcNow.ToDateOnly();
        var rankingsResult = await innSdkClient.RankingsService.GetPlayerRankingAsync(gameWorld, rankingType);
        var rankings = new List<PlayerRank>();
        rankings.AddRange(rankingsResult.Value.Top100.Select(x => x));
        rankings.AddRange(rankingsResult.Value.SurroundingRanking.Select(x => x));

        foreach (var playerRank in rankings)
        {
            var r = await playerService.AddPlayerAsync(gameWorld.Id, playerRank);
            r.LogIfFailed<RankingUpdateOrchestrator>();
        }

        return await rankingService.UpsertRankingsAsync(gameWorld.Id, now, rankings, rankingType);
    }
}

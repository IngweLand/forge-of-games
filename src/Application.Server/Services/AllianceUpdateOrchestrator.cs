using FluentResults;
using FluentResults.Extensions;
using Ingweland.Fog.Application.Server.Errors;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Errors;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Shared.Extensions;
using LazyCache;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services;

public class AllianceUpdateOrchestrator(
    IInGameAllianceService inGameAllianceService,
    IFogAllianceService fogAllianceService,
    IFogPlayerService playerService,
    IAppCache appCache,
    ICacheKeyFactory cacheKeyFactory,
    IFogDbContext context,
    ILogger<AllianceUpdateOrchestrator> logger) : IAllianceUpdateOrchestrator
{
    public async Task<Result> UpdateMembersAsync(int id, CancellationToken ct)
    {
        logger.LogDebug("Starting UpdateMembersAsync for alliance id {AllianceId}", id);

        var existingAlliance = await context.Alliances.FindAsync(id, ct);
        if (existingAlliance == null)
        {
            return Result.Fail(new FogAllianceNotFoundError(id));
        }

        logger.LogDebug("Found alliance {AllianceId} with name {AllianceName} in world {WorldId}",
            existingAlliance.InGameAllianceId, existingAlliance.Name, existingAlliance.WorldId);

        var updateMembersResult = await inGameAllianceService.GetMembersAsync(existingAlliance.Key)
            .Bind(async members =>
            {
                logger.LogDebug("Fetched {MemberCount} members from in-game alliance service for alliance {AllianceId}",
                    members.Count, existingAlliance.InGameAllianceId);

                return await playerService
                    .UpsertPlayersAsync(existingAlliance.WorldId, members)
                    .Bind(async membersWithPlayers =>
                    {
                        logger.LogDebug("Upserted {UpsertedCount} players for alliance {AllianceId}",
                            membersWithPlayers.Count, existingAlliance.InGameAllianceId);

                        var updateMembersResult =
                            await fogAllianceService.UpdateMembersAsync(existingAlliance.Key, membersWithPlayers);
                        return updateMembersResult.IsSuccess ? membersWithPlayers.ToResult() : updateMembersResult;
                    })
                    .Bind(async membersWithPlayers =>
                    {
                        var rankingPoints = membersWithPlayers.Sum(x => x.AllianceMember.RankingPoints);
                        var updatePointsResult = await fogAllianceService.UpdateRanking(existingAlliance.Id,
                            rankingPoints, DateTime.UtcNow.ToDateOnly());
                        return updatePointsResult.IsSuccess ? membersWithPlayers.ToResult() : updatePointsResult;
                    })
                    .Bind(membersWithPlayers =>
                    {
                        appCache.Remove(cacheKeyFactory.Alliance(existingAlliance.Id));
                        foreach (var t in membersWithPlayers)
                        {
                            appCache.Remove(cacheKeyFactory.Player(t.Player.Id));
                        }

                        return Result.Ok();
                    });
            });

        if (updateMembersResult.HasError<AllianceNotFoundError>())
        {
            var allianceSearchResult = await inGameAllianceService.GetAllianceAsync(existingAlliance.Key);

            if (allianceSearchResult.HasError<HohSoftError>(x => x.Error == SoftErrorType.AllianceNotFound))
            {
                logger.LogInformation("Alliance {AllianceId} not found, marking as Missing",
                    existingAlliance.InGameAllianceId);

                existingAlliance.Status = InGameEntityStatus.Missing;
                await context.SaveChangesAsync(ct);
            }
            else if (allianceSearchResult.IsFailed)
            {
                return allianceSearchResult.ToResult();
            }
            else
            {
                logger.LogDebug("Alliance {AllianceId} found.", existingAlliance.InGameAllianceId);
            }
        }

        logger.LogDebug("Completed UpdateMembersAsync for alliance id {AllianceId} with result status: {Status}",
            id, updateMembersResult.IsSuccess ? "Success" : "Failed");

        return updateMembersResult;
    }
}

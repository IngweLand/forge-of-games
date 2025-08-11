using AutoMapper;
using FluentResults.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Errors;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class TopAllianceMemberUpdateManager(
    IGameWorldsProvider gameWorldsProvider,
    IFogDbContext context,
    IInGameAllianceService inGameAllianceService,
    IFogAllianceService fogAllianceService,
    DatabaseWarmUpService databaseWarmUpService,
    IFogPlayerService playerService,
    IMapper mapper,
    ILogger<PlayersUpdateManager> logger) : ITopAllianceMemberUpdateManager
{
    private const int TOP_ALLIANCE_RANK_LIMIT = 20;

    public async Task RunAsync()
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        logger.LogDebug("Database warm-up completed");

        foreach (var gameWorld in gameWorldsProvider.GetGameWorlds())
        {
            if (gameWorld.Server == "un")
            {
                continue;
            }

            var alliances = await GetAlliances(gameWorld.Id);
            logger.LogInformation("Retrieved {PlayerCount} alliances to process", alliances.Count);
            foreach (var alliance in alliances)
            {
                logger.LogDebug("Processing alliance {@allianceKey}", alliance.Key);
                var delayTask = Task.Delay(1000);
                var getMembersResult = await inGameAllianceService.GetMembersAsync(alliance.Key);
                getMembersResult.LogIfFailed<TopAllianceMemberUpdateManager>();
                if (getMembersResult.IsSuccess)
                {
                    var updateResult = await playerService
                        .UpsertPlayersAsync(alliance.WorldId, getMembersResult.Value)
                        .Bind(players =>
                        {
                            var membersWithPlayers = getMembersResult.Value
                                .Select(x => (x, players.First(y => y.Id == x.Player.Id)))
                                .ToList();
                            return fogAllianceService.UpdateMembersAsync(alliance.Key, membersWithPlayers);
                        });
                    updateResult.LogIfFailed<TopAllianceMemberUpdateManager>();
                }
                else if (getMembersResult.HasError<AllianceNotFoundError>())
                {
                    var allianceSearchResult =
                        await inGameAllianceService.SearchAlliancesAsync(alliance.WorldId, alliance.Name);
                    allianceSearchResult.LogIfFailed<TopAllianceMemberUpdateManager>();
                    if (allianceSearchResult.IsSuccess && allianceSearchResult.Value.FirstOrDefault(x =>
                            x.Alliance.Name == alliance.Name && x.Alliance.Id == alliance.InGameAllianceId) == null)
                    {
                        alliance.Status = InGameEntityStatus.Missing;
                        await context.SaveChangesAsync();
                        logger.LogInformation("Updated alliance status for {@key} to {@status}.", alliance.Key,
                            alliance.Status);
                    }
                }

                await delayTask;
            }
        }
    }

    private Task<List<Alliance>> GetAlliances(string worldId)
    {
        logger.LogDebug("Getting alliances from the database for world {worldId}.", worldId);

        return context.Alliances
            .Where(x => x.WorldId == worldId && x.Status == InGameEntityStatus.Active)
            .OrderByDescending(x => x.RankingPoints)
            .Take(TOP_ALLIANCE_RANK_LIMIT)
            .ToListAsync();
    }
}

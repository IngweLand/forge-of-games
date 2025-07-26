using FluentResults;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class InGamePlayerService(
    IGameWorldsProvider gameWorldsProvider,
    IInnSdkClient innSdkClient,
    ILogger<InGamePlayerService> logger) : IInGamePlayerService
{
    public async Task<Result<PlayerProfile>> FetchProfile(PlayerKey playerKey)
    {
        var gw = gameWorldsProvider.GetGameWorlds().FirstOrDefault(x => x.Id == playerKey.WorldId);
        if (gw == null)
        {
            return Result.Fail<PlayerProfile>($"Could not find game world with id {playerKey.WorldId}");
        }

        logger.LogDebug("Calling PlayerService.GetPlayerProfileAsync for player {@PlayerKey}", playerKey);
        return await innSdkClient.PlayerService.GetPlayerProfileAsync(gw, playerKey.InGamePlayerId);
    }
}

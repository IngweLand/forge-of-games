using FluentResults;
using Google.Protobuf;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Inn.Models.Hoh.Errors;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Constants;
using Ingweland.Fog.InnSdk.Hoh.Errors;
using Ingweland.Fog.InnSdk.Hoh.Net.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.InnSdk.Hoh.Services;

public class PlayerService(
    IDataParsingService dataParsingService,
    IGameApiClient apiClient,
    ILogger<PlayerService> logger) : IPlayerService
{
    public Task<Result<byte[]>> GetPlayerProfileRawDataAsync(GameWorldConfig world, int playerId)
    {
        logger.LogInformation("Fetching player {@Data}", new {world.Id, playerId});
        var payload = new GetPlayerProfileRequestDto
        {
            PlayerId = playerId,
        };
        return Result.Try(
            () => apiClient.SendForProtobufAsync(world, GameEndpoints.PlayerProfilePath, payload.ToByteArray()),
            e => new NetworkError($"Failed to fetch player profile for player ID {playerId} in world {world.Id}", e));
    }

    public async Task<Result<PlayerProfile>> GetPlayerProfileAsync(GameWorldConfig world, int playerId)
    {
        var rawResult = await GetPlayerProfileRawDataAsync(world, playerId);
        if (rawResult.IsFailed)
        {
            return rawResult.ToResult();
        }

        var parseResult = dataParsingService.ParsePlayerProfile(rawResult.Value);

        return parseResult.HasError<HohInvalidCardinalityError>()
            ? Result.Fail<PlayerProfile>(new PlayerNotFoundError(playerId, world.Id))
            : parseResult;
    }
}

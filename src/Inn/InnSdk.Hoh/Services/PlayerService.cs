using Google.Protobuf;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Constants;
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
    public Task<byte[]> GetPlayerProfileRawDataAsync(GameWorldConfig world, int playerId)
    {
        logger.LogInformation("Fetching player {@Data}", new {world.Id, playerId});
        var payload = new GetPlayerProfileRequestDto
        {
            PlayerId = playerId,
        };
        return apiClient.SendForProtobufAsync(world, GameEndpoints.PlayerProfilePath, payload.ToByteArray());
    }

    public async Task<PlayerProfile> GetPlayerProfileAsync(GameWorldConfig world, int playerId)
    {
        var data = await GetPlayerProfileRawDataAsync(world, playerId);
        return dataParsingService.ParsePlayerProfile(data);
    }
}

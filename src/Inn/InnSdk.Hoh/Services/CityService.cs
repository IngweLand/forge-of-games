using Google.Protobuf;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Constants;
using Ingweland.Fog.InnSdk.Hoh.Net.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.InnSdk.Hoh.Services;

public class CityService(
    IGameApiClient apiClient,
    ILogger<RankingsService> logger) : ICityService
{
    public Task<byte[]> GetOtherCityRawDataAsync(GameWorldConfig world, int playerId)
    {
        logger.LogInformation("Fetching other city from {WorldId} for {PlayerId}", world.Id, playerId);
        var payload = new VisitCityRequestDto
        {
            PlayerId = playerId,
            CityId = "City_Capital",
        };
        return apiClient.SendForProtobufAsync(world, GameEndpoints.VisitCityPath, payload.ToByteArray());
    }
}

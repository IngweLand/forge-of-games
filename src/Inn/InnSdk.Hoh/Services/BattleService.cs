using AutoMapper;
using Google.Protobuf;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Constants;
using Ingweland.Fog.InnSdk.Hoh.Net.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.InnSdk.Hoh.Services;

public class BattleService(
    IGameApiClient apiClient,
    IDataParsingService dataParsingService,
    IMapper mapper,
    ILogger<BattleService> logger) : IBattleService
{
    public Task<byte[]> GetBattleStatsRawDataAsync(GameWorldConfig world, byte[] battleId)
    {
        logger.LogInformation("Fetching battle stats from {WorldId}", world.Id);
        var payload = mapper.Map<BattleStatsRequestDto>(battleId);
        return apiClient.SendForProtobufAsync(world, GameEndpoints.BattleStatsPath, payload.ToByteArray());
    }
    
    public async Task<BattleStats> GetBattleStatsAsync(GameWorldConfig world, byte[] battleId)
    {
        var data = await GetBattleStatsRawDataAsync(world, battleId);
        return dataParsingService.ParseBattleStats(data);
    }
}

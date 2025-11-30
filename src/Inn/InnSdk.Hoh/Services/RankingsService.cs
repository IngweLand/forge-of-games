using AutoMapper;
using FluentResults;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Constants;
using Ingweland.Fog.InnSdk.Hoh.Errors;
using Ingweland.Fog.InnSdk.Hoh.Factories.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Net.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Microsoft.Extensions.Logging;
using PlayerRankingType = Ingweland.Fog.Models.Hoh.Enums.PlayerRankingType;

namespace Ingweland.Fog.InnSdk.Hoh.Services;

public class RankingsService(
    IGameApiClient apiClient,
    IPlayerRankingRequestPayloadFactory playerRankingRequestPayloadFactory,
    IAllianceRankingRequestPayloadFactory allianceRankingRequestPayloadFactory,
    IDataParsingService dataParsingService,
    IMapper mapper,
    ILogger<RankingsService> logger) : IRankingsService
{
    public async Task<AllianceRanks> GetAllianceRankingAsync(GameWorldConfig world, AllianceRankingType rankingType)
    {
        var data = await GetAllianceRankingRawDataAsync(world, rankingType);
        return dataParsingService.ParseAllianceRankings(data);
    }

    public Task<byte[]> GetAllianceRankingRawDataAsync(GameWorldConfig world, AllianceRankingType rankingType)
    {
        if (rankingType == AllianceRankingType.Undefined)
        {
            logger.LogError("Invalid alliance ranking type: {RankingType}", rankingType);
            throw new ArgumentOutOfRangeException(nameof(rankingType),
                $"Alliance ranking type cannot be {nameof(AllianceRankingType.Undefined)}");
        }

        logger.LogInformation("Fetching alliance ranking from {WorldId}", world.Id);
        return apiClient.SendForProtobufAsync(world, GameEndpoints.AllianceRankingPath,
            allianceRankingRequestPayloadFactory.Create(rankingType));
    }

    public Task<Result<byte[]>> GetPlayerRankingRawDataAsync(GameWorldConfig world, PlayerRankingType rankingType)
    {
        logger.LogInformation("Fetching player ranking from {WorldId}", world.Id);

        if (!Enum.IsDefined(typeof(Inn.Models.Hoh.PlayerRankingType), (int) rankingType))
        {
            return Task.FromResult(Result.Fail<byte[]>(
                new EnumValueNotDefinedError<Inn.Models.Hoh.PlayerRankingType>((int) rankingType)));
        }

        return Result.Try(
            () => apiClient.SendForProtobufAsync(world, GameEndpoints.PlayerRankingPath,
                playerRankingRequestPayloadFactory.Create((Inn.Models.Hoh.PlayerRankingType) (int) rankingType)),
            e => new GameApiClientError("Failed to fetch player ranking", e));
    }

    public async Task<Result<PlayerRanks>> GetPlayerRankingAsync(GameWorldConfig world, PlayerRankingType rankingType)
    {
        var rawResult = await GetPlayerRankingRawDataAsync(world, rankingType);
        return rawResult.IsFailed ? rawResult.ToResult() : dataParsingService.ParsePlayerRankings(rawResult.Value);
    }
}

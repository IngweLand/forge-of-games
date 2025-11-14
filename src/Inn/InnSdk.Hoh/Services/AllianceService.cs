using FluentResults;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Inn.Models.Hoh.Errors;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Constants;
using Ingweland.Fog.InnSdk.Hoh.Errors;
using Ingweland.Fog.InnSdk.Hoh.Net.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.InnSdk.Hoh.Services;

public class AllianceService(
    IDataParsingService dataParsingService,
    IGameApiClient apiClient,
    ILogger<AllianceService> logger) : IAllianceService
{
    public async Task<Result<IReadOnlyCollection<AllianceWithLeader>>> SearchAlliancesAsync(GameWorldConfig world,
        string searchString)
    {
        logger.LogInformation("Searching alliance {@Data}", new {WorldId = world.Id, searchString});
        var payload = new SearchAllianceRequestDto
        {
            SearchString = searchString,
        };
        var rawResult = await Result.Try(
            () => apiClient.SendForProtobufAsync(world, GameEndpoints.AllianceSearchPath, payload.ToByteArray()),
            e => new NetworkError(
                $"Failed to perform an alliance search with search string {searchString} in world {world.Id}", e));

        if (rawResult.IsFailed)
        {
            return rawResult.ToResult();
        }

        return dataParsingService.ParseSearchAllianceResponse(rawResult.Value);
    }

    public Task<Result<byte[]>> GetMembersRawDataAsync(GameWorldConfig world, int allianceId)
    {
        logger.LogInformation("Getting alliance's members {@Data}", new {WorldId = world.Id, Id = allianceId});
        var payload = new AllianceRequest
        {
            Id = allianceId,
        };
        return Result.Try(
            () => apiClient.SendForProtobufAsync(world, GameEndpoints.AllianceMembersPath, payload.ToByteArray()),
            e => new NetworkError($"Failed to get members of the alliance with id {allianceId} in world {world.Id}",
                e));
    }

    public Task<Result<byte[]>> GetAllianceRawDataAsync(GameWorldConfig world, int allianceId)
    {
        logger.LogInformation("Getting alliance {@Data}", new {WorldId = world.Id, Id = allianceId});
        var payload = new AllianceRequest
        {
            Id = allianceId,
        };
        return Result.Try(
            () => apiClient.SendForProtobufAsync(world, GameEndpoints.AlliancePath, payload.ToByteArray()),
            e => new NetworkError($"Failed to get alliance with id {allianceId} in world {world.Id}",
                e));
    }

    public async Task<Result<AllianceWithLeader>> GetAllianceAsync(GameWorldConfig world, int allianceId)
    {
        var rawResult = await GetAllianceRawDataAsync(world, allianceId);

        if (rawResult.IsFailed)
        {
            return rawResult.ToResult();
        }

        return dataParsingService.ParseAllianceWithLeader(rawResult.Value);
    }

    // TODO: this is WIP. We should return parsed list of alliances.
    public async Task<Result<BatchResponse>> GetAlliancesAsync(GameWorldConfig world, HashSet<int> allianceIds)
    {
        logger.LogInformation("Getting alliances {@Data}",
            new {WorldId = world.Id, Ids = string.Join(",", allianceIds)});
        var batchItems = new List<BatchRequestItemDto>();
        foreach (var allianceId in allianceIds)
        {
            var guid = Guid.NewGuid();
            var batchItem = new BatchRequestItemDto
            {
                Uuid = guid.ToString(),
                WrappedUuid = guid.ToString(),
                Endpoint = GameEndpoints.AlliancePath,
                Payload = Any.Pack(new AllianceRequest {Id = allianceId}),
                UpdatedAt = Timestamp.FromDateTime(DateTime.UtcNow),
            };
            batchItems.Add(batchItem);
        }

        var payload = new BatchRequestDto();
        payload.Items.AddRange(batchItems);
        var rawResult = await Result.Try(
            () => apiClient.SendForProtobufAsync(world, GameEndpoints.BatchPath, payload.ToByteArray()),
            e => new NetworkError($"Failed to get alliances in world {world.Id}", e));

        if (rawResult.IsFailed)
        {
            return rawResult.ToResult();
        }

        return dataParsingService.ParseBatchResponse(rawResult.Value);
    }

    public async Task<Result<IReadOnlyCollection<AllianceMember>>> GetMembersAsync(GameWorldConfig world,
        int allianceId)
    {
        var rawResult = await GetMembersRawDataAsync(world, allianceId);
        if (rawResult.IsFailed)
        {
            return rawResult.ToResult();
        }

        var parseResult = dataParsingService.ParseAllianceMembersResponse(rawResult.Value);

        return parseResult.HasError<HohSoftError>(x => x.Error == SoftErrorType.AllianceNotFound) ||
            parseResult.HasError<HohInvalidCardinalityError>()
                ? Result.Fail<IReadOnlyCollection<AllianceMember>>(new AllianceNotFoundError(allianceId, world.Id))
                : parseResult.Value.Members.ToResult();
    }
}

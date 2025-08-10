using FluentResults;
using Google.Protobuf;
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
    public async Task<Result<IReadOnlyCollection<AllianceSearchResult>>> SearchAlliancesAsync(GameWorldConfig world,
        string searchString)
    {
        logger.LogInformation("Searching alliance {@Data}", new {world.Id, searchString});
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

        var parseResult = dataParsingService.ParseSearchAllianceResponse(rawResult.Value);

        return parseResult.HasError<HohInvalidCardinalityError>()
            ? Result.Fail<IReadOnlyCollection<AllianceSearchResult>>(
                new SearchAllianceResponseNotFoundError(searchString, world.Id))
            : parseResult;
    }

    public Task<Result<byte[]>> GetMembersRawDataAsync(GameWorldConfig world, int allianceId)
    {
        logger.LogInformation("Getting alliance's members {@Data}", new {world.Id, id = allianceId});
        var payload = new AllianceMembersRequestDto
        {
            Id = allianceId,
        };
        return Result.Try(
            () => apiClient.SendForProtobufAsync(world, GameEndpoints.AllianceMembersPath, payload.ToByteArray()),
            e => new NetworkError($"Failed to get members of the alliance with id {allianceId} in world {world.Id}",
                e));
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

        return parseResult.HasError<HohInvalidCardinalityError>()
            ? Result.Fail<IReadOnlyCollection<AllianceMember>>(new AllianceNotFoundError(allianceId, world.Id))
            : parseResult.Value.Members.ToResult();
    }
}

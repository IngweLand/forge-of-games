using FluentResults;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class InGameAllianceService(
    IGameWorldsProvider gameWorldsProvider,
    IInnSdkClient innSdkClient,
    ILogger<InGameAllianceService> logger) : IInGameAllianceService
{
    public async Task<Result<IReadOnlyCollection<AllianceMember>>> GetMembersAsync(AllianceKey allianceKey)
    {
        var gw = gameWorldsProvider.GetGameWorlds().FirstOrDefault(x => x.Id == allianceKey.WorldId);
        if (gw == null)
        {
            return Result.Fail<IReadOnlyCollection<AllianceMember>>($"Could not find game world with id {
                allianceKey.WorldId}");
        }

        logger.LogDebug("Calling {method} for alliance {@allianceKey}",
            $"{nameof(InGameAllianceService)}.{nameof(GetMembersAsync)}", allianceKey);
        return await innSdkClient.AllianceService.GetMembersAsync(gw, allianceKey.InGameAllianceId);
    }

    public async Task<Result<IReadOnlyCollection<AllianceWithLeader>>> SearchAlliancesAsync(string worldId,
        string searchString)
    {
        var gw = gameWorldsProvider.GetGameWorlds().FirstOrDefault(x => x.Id == worldId);
        if (gw == null)
        {
            return Result.Fail<IReadOnlyCollection<AllianceWithLeader>>(
                $"Could not find game world with id {worldId}");
        }

        logger.LogDebug("Calling {method} with search string {searchString}",
            $"{nameof(InGameAllianceService)}.{nameof(SearchAlliancesAsync)}", searchString);

        return await innSdkClient.AllianceService.SearchAlliancesAsync(gw, searchString);
    }

    public async Task<Result<AllianceWithLeader>> GetAllianceAsync(AllianceKey allianceKey)
    {
        var gw = gameWorldsProvider.GetGameWorlds().FirstOrDefault(x => x.Id == allianceKey.WorldId);
        if (gw == null)
        {
            return Result.Fail<AllianceWithLeader>($"Could not find game world with id {allianceKey.WorldId}");
        }

        logger.LogDebug("Calling {method} for alliance {@allianceKey}",
            $"{nameof(InGameAllianceService)}.{nameof(GetAllianceAsync)}", allianceKey);
        return await innSdkClient.AllianceService.GetAllianceAsync(gw, allianceKey.InGameAllianceId);
    }
}

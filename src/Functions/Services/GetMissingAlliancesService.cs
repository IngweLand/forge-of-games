using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Functions.Functions;
using Ingweland.Fog.Functions.Services.Orchestration;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IGetMissingAlliancesService
{
    Task RunAsync();
}

public class GetMissingAlliancesService(
    IFogDbContext context,
    IInnSdkClient innSdkClient,
    IMapper mapper,
    IGameWorldsProvider gameWorldsProvider,
    IAllianceUpdateOrchestrator allianceUpdateOrchestrator,
    ILogger<ManualTriggerFunction> logger) : IGetMissingAlliancesService
{
    public async Task RunAsync()
    {
        var gw = gameWorldsProvider.GetGameWorlds().First(x => x.Id == "zz1");
        var allianceIds = await context.Alliances.ProjectTo<AllianceKey>(mapper.ConfigurationProvider).ToListAsync();
        var ids = allianceIds.Where(x => x.WorldId == gw.Id).Select(x => x.InGameAllianceId).ToList();
        var allianceIdsSet = ids.ToHashSet();
        var newAlliances = new List<AllianceWithLeader>();
        for (var i = ids.Max(); i > 0; i--)
        {
            if (allianceIdsSet.Contains(i))
            {
                continue;
            }

            try
            {
                await Task.Delay(500);

                var a = await innSdkClient.AllianceService.GetAllianceAsync(gw, i);
                if (a.IsSuccess)
                {
                    logger.LogInformation(">>> Fetched alliance {i} for world {gw}", i, gw.Id);
                    newAlliances.Add(a.Value);
                }
                else
                {
                    continue;
                }
            }
            catch (Exception e)
            {
                continue;
            }

            if (newAlliances.Count > 5)
            {
                await AddAlliancesAsync(newAlliances, gw.Id);
                var newAllianceIds =
                    await GetExistingAlliancesAsync(newAlliances.Select(x => x.Alliance.Id).ToHashSet(), gw.Id);
                foreach (var id in newAllianceIds.Select(x => x.Id))
                {
                    var delayTask = Task.Delay(200);
                    var result =
                        await allianceUpdateOrchestrator.UpdateMembersAsync(id, CancellationToken.None);
                    result.LogIfFailed<AllianceMembersUpdateManager>();

                    await delayTask;
                }

                newAlliances.Clear();
                logger.LogInformation("==== Last saved alliance: {aId}:{wId} ====", i, gw.Id);
            }
        }

        if (newAlliances.Count > 0)
        {
            await AddAlliancesAsync(newAlliances, gw.Id);
            var newAllianceIds =
                await GetExistingAlliancesAsync(newAlliances.Select(x => x.Alliance.Id).ToHashSet(), gw.Id);
            foreach (var id in newAllianceIds.Select(x => x.Id))
            {
                var delayTask = Task.Delay(200);
                var result =
                    await allianceUpdateOrchestrator.UpdateMembersAsync(id, CancellationToken.None);
                result.LogIfFailed<AllianceMembersUpdateManager>();

                await delayTask;
            }
        }

        logger.LogInformation("DONE");
    }

    private async Task AddAlliancesAsync(IEnumerable<AllianceWithLeader> alliances, string worldId)
    {
        var today = DateTime.UtcNow.ToDateOnly();
        var now = DateTime.UtcNow;
        var unique = alliances
            .DistinctBy(p => p.Alliance.Id)
            .ToDictionary(p => p.Alliance.Id);
        logger.LogInformation("{ValidCount} valid alliances after filtering", unique.Count);
        var existingAlliances =
            await GetExistingAlliancesAsync(unique.Keys.ToHashSet(), worldId);
        var newAllianceKeys =
            unique.Keys.ToHashSet().Except(existingAlliances.Select(x => x.InGameAllianceId)).ToList();
        var newAlliances = newAllianceKeys.Select(k =>
        {
            var alliance = unique[k];
            return new Alliance
            {
                WorldId = worldId,
                InGameAllianceId = alliance.Alliance.Id,
                Name = alliance.Alliance.Name,
                AvatarIconId = alliance.Alliance.AvatarIconId,
                AvatarBackgroundId = alliance.Alliance.AvatarBackgroundId,
                Rank = alliance.Alliance.Rank,
                UpdatedAt = today,
                NameHistory = new List<AllianceNameHistoryEntry>
                    {new() {Name = alliance.Alliance.Name, ChangedAt = now}},
            };
        }).ToList();

        if (newAlliances.Count > 0)
        {
            context.Alliances.AddRange(newAlliances);
            await context.SaveChangesAsync();
        }

        logger.LogInformation("SaveChangesAsync completed, added {AddedAllianceCount} alliances", newAlliances.Count);
    }

    private async Task<IReadOnlyCollection<Alliance>> GetExistingAlliancesAsync(HashSet<int> inGameAllianceIds,
        string worldId)
    {
        var alliances = await context.Alliances
            .Where(p => inGameAllianceIds.Contains(p.InGameAllianceId))
            .ToListAsync();

        return alliances.Where(x => x.WorldId == worldId).ToList();
    }
}

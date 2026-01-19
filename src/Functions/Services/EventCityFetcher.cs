using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IEventCityFetcher
{
    Task RunAsync();
}

public class EventCityFetcher(
    IFogDbContext context,
    IGameWorldsProvider gameWorldsProvider,
    IPlayerCityService playerCityService,
    IMapper mapper,
    ILogger<EventCityFetcher> logger) : IEventCityFetcher
{
    private const int MAX_EVENT_CITY_RANKINGS = 100;
    private const int MAX_FAILED_FETCHES = 5;

    private static readonly TimeSpan FailedFetchLastTryTime = TimeSpan.FromHours(3);

    private static readonly IReadOnlyDictionary<string, int> TopAllianceRankLimits = new Dictionary<string, int>
    {
        {"un1", 30},
    };

    private static readonly IReadOnlyDictionary<TimeSpan, int> FetchIntervalByElapsedTime =
        new Dictionary<TimeSpan, int>
        {
            {TimeSpan.FromHours(3), 15},
            {TimeSpan.FromHours(12), 30},
            {TimeSpan.FromHours(48), 60},
            {TimeSpan.MaxValue, 120},
        };

    public async Task RunAsync()
    {
        foreach (var kvp in TopAllianceRankLimits)
        {
            var gw = gameWorldsProvider.Get(kvp.Key).Value;
            var currentEvent = await GetCurrentEvent(gw.Id);
            if (currentEvent == null)
            {
                logger.LogInformation("Allied culture event data not available for {WorldId}. Skipping.", gw.Id);
                return;
            }

            var wonderId = Enum.GetValues<WonderId>()
                .FirstOrDefault(x => currentEvent.InGameDefinitionId.EndsWith(x.ToString()));
            if (wonderId == WonderId.Undefined)
            {
                throw new InvalidOperationException($"No wonder found for event {currentEvent.Id}.");
            }

            await CreateFetchPlanIfRequired(currentEvent);

            var i = 0;
            var fetchedCount = 0;
            while (true)
            {
                var fetchState = await context.EventCityFetchStates
                    .Where(x => x.EventId == currentEvent.Id)
                    .OrderBy(x => x.Id)
                    .Skip(i)
                    .Take(1)
                    .FirstOrDefaultAsync();
                if (fetchState == null)
                {
                    break;
                }

                var eventElapsedTime = DateTime.UtcNow - currentEvent.StartAt;
                if (fetchState.FailuresCount >= MAX_FAILED_FETCHES && eventElapsedTime < FailedFetchLastTryTime)
                {
                    continue;
                }

                if (fetchState.FetchTimestamps.Count > 0 &&
                    !ShouldFetch(currentEvent.StartAt, fetchState.FetchTimestamps.Order().Last()))
                {
                    i++;
                    logger.LogDebug("Skipping player {PlayerId}", fetchState.PlayerId);
                    continue;
                }

                var delayTask = Task.Delay(300);

                var fetched = await FetchCity(currentEvent.Id, fetchState.PlayerId, fetchState.GameWorldId, fetchState.InGamePlayerId,
                    wonderId.ToCity());
                if (fetched)
                {
                    fetchState.FetchTimestamps.Add(DateTime.UtcNow);
                    fetchState.FailuresCount = 0;
                    fetchedCount++;
                }
                else
                {
                    if (fetchState.FailuresCount >= MAX_FAILED_FETCHES)
                    {
                        context.EventCityFetchStates.Remove(fetchState);
                    }
                    else
                    {
                        fetchState.FailuresCount++;
                    }
                }

                await context.SaveChangesAsync();
                await delayTask;

                i++;
            }

            logger.LogInformation("Processed {count} players for world {worldId}. Fetched {fetchedCount} cities", i,
                gw.Id, fetchedCount);
        }
    }

    private static bool ShouldFetch(DateTime eventStartTime, DateTime lastFetchTimestamp)
    {
        var now = DateTime.UtcNow;
        var eventElapsedTime = now - eventStartTime;
        var interval = FetchIntervalByElapsedTime.Last().Value;
        foreach (var kvp in FetchIntervalByElapsedTime)
        {
            if (eventElapsedTime <= kvp.Key)
            {
                interval = kvp.Value;
                break;
            }
        }

        return lastFetchTimestamp < now.AddMinutes(-interval);
    }

    private async Task<IReadOnlyCollection<PlayerKeyExtended>> GetInGamePlayers(string worldId)
    {
        var members = await context.Alliances
            .Include(x => x.Members)
            .ThenInclude(x => x.Player)
            .Where(x => x.WorldId == worldId && x.Status == InGameEntityStatus.Active)
            .OrderByDescending(x => x.RankingPoints)
            .Take(TopAllianceRankLimits[worldId])
            .SelectMany(x => x.Members.Select(y => y.Player))
            .ProjectTo<PlayerKeyExtended>(mapper.ConfigurationProvider)
            .ToHashSetAsync();
        var topEventPlayers = await context.PlayerRankings
            .Include(x => x.Player)
            .Where(x => x.Type == PlayerRankingType.EventCityProgress && x.Player.WorldId == worldId)
            .OrderByDescending(x => x.Points)
            .Take(MAX_EVENT_CITY_RANKINGS)
            .Select(x => x.Player)
            .ProjectTo<PlayerKeyExtended>(mapper.ConfigurationProvider)
            .ToListAsync();
        return members.Union(topEventPlayers).ToList();
    }

    private async Task<bool> FetchCity(int inGameEventId, int playerId, string worldId, int inGamePlayerId, CityId cityId)
    {
        var fetchedCity = await playerCityService.FetchCityAsync(worldId, inGamePlayerId, cityId);
        if (fetchedCity == null)
        {
            logger.LogWarning("Failed to fetch city for player {PlayerId} from world {WorldId}",
                playerId, worldId);
            return false;
        }

        var savedCity = await playerCityService.SaveEventCityAsync(inGameEventId, playerId, fetchedCity);
        if (savedCity == null)
        {
            logger.LogError("Failed to save city for player {PlayerId} from world {WorldId}",
                playerId, worldId);
            return false;
        }

        logger.LogDebug("Successfully processed city for player {PlayerId} from world {WorldId}",
            playerId, worldId);
        return true;
    }

    private Task<InGameEventEntity?> GetCurrentEvent(string worldId)
    {
        var now = DateTime.UtcNow;
        return context.InGameEvents.FirstOrDefaultAsync(x =>
            x.DefinitionId == EventDefinitionId.EventCity && x.WorldId == worldId && x.StartAt <= now &&
            x.EndAt >= now);
    }

    private async Task CreateFetchPlanIfRequired(InGameEventEntity currentEvent)
    {
        if (await context.EventCityFetchStates.Where(x => x.EventId == currentEvent.Id).AnyAsync())
        {
            return;
        }

        var players = await GetInGamePlayers(currentEvent.WorldId);
        var states = players.Select(x => new EventCityFetchState
            {
                EventId = currentEvent.Id,
                PlayerId = x.Id,
                InGamePlayerId = x.InGamePlayerId,
                GameWorldId = x.WorldId,
            })
            .ToList();

        context.EventCityFetchStates.AddRange(states);
        await context.SaveChangesAsync();
    }
}

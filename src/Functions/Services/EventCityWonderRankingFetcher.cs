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

public interface IEventCityWonderRankingFetcher
{
    Task<bool> RunAsync();
}

public class EventCityWonderRankingFetcher(
    IFogDbContext context,
    IGameWorldsProvider gameWorldsProvider,
    IPlayerCityService playerCityService,
    IHohCityCreationService cityCreationService,
    ILogger<EventCityWonderRankingFetcher> logger) :EventCityFetcherBase (context), IEventCityWonderRankingFetcher
{
    private const int BATCH_SIZE = 400;

    private static readonly IReadOnlyDictionary<string, int> TopAllianceRankLimits = new Dictionary<string, int>
    {
        {"un1", 100},
        {"zz1", 30},
    };

    public async Task<bool> RunAsync()
    {
        var shouldRunAgain = false;
        foreach (var gw in gameWorldsProvider.GetGameWorlds())
        {
            var currentEvent = await GetCurrentEvent(gw.Id);
            if (currentEvent == null)
            {
                continue;
            }

            var wonderId = Enum.GetValues<WonderId>()
                .FirstOrDefault(x => currentEvent.InGameDefinitionId.EndsWith(x.ToString()));
            if (wonderId == WonderId.Undefined)
            {
                continue;
            }

            var players = await GetInGamePlayerIds(gw.Id, currentEvent.Id);
            foreach (var player in players.Take(BATCH_SIZE))
            {
                var delayTask = Task.Delay(300);
                var fetchedCity =
                    await playerCityService.FetchCityAsync(gw.Id, player.InGamePlayerId, wonderId.ToCity());
                if (fetchedCity == null)
                {
                    await delayTask;
                    continue;
                }

                try
                {
                    var city = await cityCreationService.Create(fetchedCity, string.Empty);
                    Context.EventCityWonderRankings.Add(new EventCityWonderRanking
                    {
                        PlayerId = player.Id,
                        InGameEventId = currentEvent.Id,
                        CollectedAt = DateTime.UtcNow,
                        WonderLevel = city.WonderLevel,
                    });

                    await Context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    logger.LogWarning(e, "Error creating city for player {PlayerId}", player.Id);
                }

                await delayTask;
            }

            shouldRunAgain = shouldRunAgain || players.Count >= BATCH_SIZE;
            
            logger.LogInformation("Processed {count} players for world {worldId}", players.Count, gw.Id);
        }

        return shouldRunAgain;
    }

    private async Task<IReadOnlyCollection<Player>> GetInGamePlayerIds(string worldId, int eventId)
    {
        var players = await Context.Alliances
            .Include(x => x.Members)
            .ThenInclude(x => x.Player)
            .ThenInclude(x => x.EventCityWonderRankings.Where(y => y.InGameEventId == eventId))
            .Where(x => x.WorldId == worldId && x.Status == InGameEntityStatus.Active)
            .OrderByDescending(x => x.RankingPoints)
            .Take(TopAllianceRankLimits[worldId])
            .SelectMany(x => x.Members.Select(y => y.Player))
            .ToListAsync();
        var cutOffTime = DateTime.UtcNow.AddHours(-1);
        var filteredPlayers = new List<Player>();
        foreach (var player in players)
        {
            var latest = player.EventCityWonderRankings.OrderByDescending(x => x.CollectedAt).FirstOrDefault();
            if (latest == null || latest.CollectedAt < cutOffTime)
            {
                filteredPlayers.Add(player);
            }
        }

        return filteredPlayers;
    }
}

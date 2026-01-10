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
    ILogger<EventCityFetcher> logger) : EventCityFetcherBase(context), IEventCityFetcher
{
    private static readonly IReadOnlyDictionary<string, int> TopAllianceRankLimits = new Dictionary<string, int>
    {
        {"un1", 30},
    };

    public async Task RunAsync()
    {
        var gw = gameWorldsProvider.Get("un1").Value;
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
            var message = $"No wonder found for event {currentEvent.Id}.";
            logger.LogError(message);
            throw new Exception(message);
        }

        var players = await GetInGamePlayers(gw.Id);
        foreach (var player in players)
        {
            var delayTask = Task.Delay(300);

            _ = await FetchCity(player, wonderId.ToCity());

            await delayTask;
        }

        logger.LogInformation("Processed {count} players for world {worldId}", players.Count, gw.Id);
    }

    private async Task<IReadOnlyCollection<PlayerKeyExtended>> GetInGamePlayers(string worldId)
    {
        return await Context.Alliances
            .Include(x => x.Members)
            .ThenInclude(x => x.Player)
            .Where(x => x.WorldId == worldId && x.Status == InGameEntityStatus.Active)
            .OrderByDescending(x => x.RankingPoints)
            .Take(TopAllianceRankLimits[worldId])
            .SelectMany(x => x.Members.Select(y => y.Player))
            .ProjectTo<PlayerKeyExtended>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    private async Task<bool> FetchCity(PlayerKeyExtended player, CityId cityId)
    {
        var fetchedCity = await playerCityService.FetchCityAsync(player.WorldId, player.InGamePlayerId, cityId);
        if (fetchedCity == null)
        {
            logger.LogWarning("Failed to fetch city for player {PlayerId} from world {WorldId}",
                player.Id, player.WorldId);
            return false;
        }

        var savedCity = await playerCityService.SaveEventCityAsync(player.Id, fetchedCity);
        if (savedCity == null)
        {
            logger.LogError("Failed to save city for player {PlayerId} from world {WorldId}",
                player.Id, player.WorldId);
            return false;
        }

        logger.LogDebug("Successfully processed city for player {PlayerId} from world {WorldId}",
            player.Id, player.WorldId);
        return true;
    }
}

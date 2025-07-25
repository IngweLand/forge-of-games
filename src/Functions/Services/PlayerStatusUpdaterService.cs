using Ingweland.Fog.Application.Server.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IPlayerStatusUpdaterService
{
    Task UpdateAsync(IEnumerable<int> removedPlayers);
}

public class PlayerStatusUpdaterService(IFogDbContext context, ILogger<PlayerStatusUpdaterService> logger)
    : IPlayerStatusUpdaterService
{
    public async Task UpdateAsync(IEnumerable<int> removedPlayers)
    {
        var unique = removedPlayers.ToHashSet();
        foreach (var id in unique)
        {
            var player = await context.Players.FindAsync(id);
            if (player == null)
            {
                continue;
            }

            player.CurrentAlliance = null;
            player.LedAlliance = null;
            player.IsPresentInGame = false;
        }

        await context.SaveChangesAsync();
    }
}

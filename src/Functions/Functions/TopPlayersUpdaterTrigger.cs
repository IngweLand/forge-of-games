using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class TopPlayersUpdaterTrigger(
    ITopPlayersUpdateManager topPlayersUpdateManager,
    ILogger<TopPlayersUpdaterTrigger> logger)
{
    [Function(nameof(TopPlayersUpdaterTrigger))]
    public Task<bool> Run([ActivityTrigger] object? _)
    {
        logger.LogInformation("{activity} started.", nameof(TopPlayersUpdaterTrigger));
        return topPlayersUpdateManager.RunAsync();
    }
}

using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class AlliancesUpdaterTrigger(
    IAlliancesUpdateManager alliancesUpdateManager,
    ILogger<AlliancesUpdaterTrigger> logger)
{
    [Function(nameof(AlliancesUpdaterTrigger))]
    public Task<bool> Run([ActivityTrigger] object? _)
    {
        logger.LogInformation("{activity} started.", nameof(AlliancesUpdaterTrigger));
        return alliancesUpdateManager.RunAsync();
    }
}

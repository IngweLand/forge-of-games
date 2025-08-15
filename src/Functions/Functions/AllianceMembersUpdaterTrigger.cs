using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class AllianceMembersUpdaterTrigger(
    IAllianceMembersUpdateManager allianceMembersUpdateManager,
    ILogger<AllianceMembersUpdaterTrigger> logger)
{
    [Function(nameof(AllianceMembersUpdaterTrigger))]
    public Task<bool> Run([ActivityTrigger] object? _)
    {
        logger.LogInformation("{activity} started.", nameof(AllianceMembersUpdaterTrigger));
        return allianceMembersUpdateManager.RunAsync();
    }
}

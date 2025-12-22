using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh;

public class InGameEventUiService(IInGameEventService inGameEventService, ILogger<InGameEventUiService> logger)
    : UiServiceBase(logger), IInGameEventUiService
{
    public async Task<WonderId> GetCurrentWonderAsync(string worldId, CancellationToken ct = default)
    {
        var result =
            await ExecuteSafeAsync(() => inGameEventService.GetCurrent(worldId, EventDefinitionId.EventCity, ct), null);
        return result == null
            ? WonderId.Undefined
            : Enum.GetValues<WonderId>().FirstOrDefault(x => result.InGameDefinitionId.EndsWith(x.ToString()));
    }
}

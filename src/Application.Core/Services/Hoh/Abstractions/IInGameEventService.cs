using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IInGameEventService
{
    [Get(FogUrlBuilder.ApiRoutes.IN_GAME_EVENTS_TEMPLATE)]
    Task<IReadOnlyCollection<InGameEventDto>> Get(string worldId, EventDefinitionId eventDefinitionId,
        CancellationToken ct = default);

    [Get(FogUrlBuilder.ApiRoutes.CURRENT_IN_GAME_EVENT_TEMPLATE)]
    Task<InGameEventDto?> GetCurrent(string worldId, EventDefinitionId eventDefinitionId,
        CancellationToken ct = default);
}

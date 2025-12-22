using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Services.Queries;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class InGameEventService(ISender sender) : IInGameEventService
{
    public Task<IReadOnlyCollection<InGameEventDto>> Get(string worldId, EventDefinitionId eventDefinitionId,
        CancellationToken ct = default)
    {
        var query = new GetEventsQuery
        {
            WorldId = worldId,
            EventDefinitionId = eventDefinitionId,
        };
        return sender.Send(query, ct);
    }

    public Task<InGameEventDto?> GetCurrent(string worldId, EventDefinitionId eventDefinitionId,
        CancellationToken ct = default)
    {
        var query = new GetCurrentInGameEventQuery(worldId, eventDefinitionId);
        return sender.Send(query, ct);
    }
}

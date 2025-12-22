using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Services.Queries;

public record GetCurrentInGameEventQuery(string WorldId, EventDefinitionId EventDefinitionId)
    : IRequest<InGameEventDto?>, ICacheableRequest
{
    public TimeSpan? Duration { get; }
    public DateTimeOffset? Expiration => DateTimeUtils.GetNextNoonUtc();
}

public class GetCurrentInGameEventQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetCurrentInGameEventQuery, InGameEventDto?>
{
    public async Task<InGameEventDto?> Handle(GetCurrentInGameEventQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var result = await context.InGameEvents.FirstOrDefaultAsync(
            x => x.DefinitionId == request.EventDefinitionId && x.WorldId == request.WorldId && x.StartAt <= now &&
                x.EndAt >= now, cancellationToken);

        return result != null ? mapper.Map<InGameEventDto>(result) : null;
    }
}

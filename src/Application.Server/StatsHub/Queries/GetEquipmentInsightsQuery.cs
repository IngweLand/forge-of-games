using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetEquipmentInsightsQuery(string UnitId)
    : IRequest<IReadOnlyCollection<EquipmentInsightsDto>>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromHours(24);
    public DateTimeOffset? Expiration { get; }
}

public class GetEquipmentInsightsQueryHandler(
    IFogDbContext context,
    IMapper mapper,
    ILogger<GetPlayerProfileQueryHandler> logger)
    : IRequestHandler<GetEquipmentInsightsQuery, IReadOnlyCollection<EquipmentInsightsDto>>
{
    public async Task<IReadOnlyCollection<EquipmentInsightsDto>> Handle(GetEquipmentInsightsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing GetEquipmentInsightsQueryHandler for UnitId: {UnitId}", request.UnitId);

        var items = await context.EquipmentInsights.Where(x => x.UnitId == request.UnitId)
            .ToListAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<EquipmentInsightsDto>>(items);
    }
}

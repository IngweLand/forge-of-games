using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.StatsHub.Queries;
using Ingweland.Fog.Dtos.Hoh.Stats;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class RelicService(ISender sender) : IRelicService
{
    public Task<IReadOnlyCollection<RelicInsightsDto>> GetInsightsAsync(string unitId,
        CancellationToken ct = default)
    {
        var query = new GetRelicInsightsQuery(unitId);
        return sender.Send(query, ct);
    }
}

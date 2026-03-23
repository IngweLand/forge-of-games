using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.StatsHub.Queries;
using Ingweland.Fog.Dtos.Hoh.Stats;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class EquipmentInsightsService(ISender sender) : IEquipmentInsightsService
{
    public Task<IReadOnlyCollection<EquipmentInsightsDto>> GetInsightsAsync(string unitId,
        CancellationToken ct = default)
    {
        var query = new GetEquipmentInsightsQuery(unitId);
        return sender.Send(query, ct);
    }
}

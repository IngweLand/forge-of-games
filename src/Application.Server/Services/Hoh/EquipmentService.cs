using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Services.Queries;
using Ingweland.Fog.Application.Server.StatsHub.Queries;
using Ingweland.Fog.Dtos.Hoh.Equipment;
using Ingweland.Fog.Dtos.Hoh.Stats;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class EquipmentService(ISender sender) : IEquipmentService
{
    public Task<IReadOnlyCollection<EquipmentInsightsDto>> GetInsightsAsync(string unitId,
        CancellationToken ct = default)
    {
        var query = new GetEquipmentInsightsQuery(unitId);
        return sender.Send(query, ct);
    }

    public Task<EquipmentDataDto> GetEquipmentData(CancellationToken ct = default)
    {
        var query = new GetEquipmentDataQuery();
        return sender.Send(query, ct);
    }
}

using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IEquipmentInsightsService
{
    [Get(FogUrlBuilder.ApiRoutes.EQUIPMENT_INSIGHTS_TEMPLATE)]
    Task<IReadOnlyCollection<EquipmentInsightsDto>> GetInsightsAsync(string unitId, CancellationToken ct = default);
}

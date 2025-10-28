using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IRelicService
{
    [Get(FogUrlBuilder.ApiRoutes.RELICS_INSIGHTS_TEMPLATE)]
    Task<IReadOnlyCollection<RelicInsightsDto>> GetInsightsAsync(string unitId, CancellationToken ct = default);
}

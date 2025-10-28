using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Relic;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface IRelicUiService
{
    Task<IReadOnlyCollection<RelicInsightsViewModel>> GetRelicInsights(string unitId, CancellationToken ct);
}

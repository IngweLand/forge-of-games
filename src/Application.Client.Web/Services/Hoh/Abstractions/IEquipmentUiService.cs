using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface IEquipmentUiService
{
    Task<IReadOnlyCollection<EquipmentItemViewModel>> GetEquipmentAsync();
    Task<IReadOnlyCollection<EquipmentSlotTypeViewModel>> GetEquipmentSlotTypesAsync();
    Task<IReadOnlyCollection<EquipmentInsightsViewModel>> GetEquipmentInsights(string unitId, CancellationToken ct);
}

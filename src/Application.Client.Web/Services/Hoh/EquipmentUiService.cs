using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh;

public class EquipmentUiService(
    IPersistenceService persistenceService,
    IMapper mapper,
    IEquipmentSlotTypeIconUrlProvider equipmentSlotTypeIconUrlProvider,
    IEquipmentService equipmentService,
    IEquipmentInsightsViewModelFactory equipmentInsightsViewModelFactory,
    IHohCoreDataCache coreDataCache) : IEquipmentUiService
{
    private static readonly List<EquipmentSlotType> SlotTypes =
    [
        EquipmentSlotType.Hand, EquipmentSlotType.Garment, EquipmentSlotType.Hat, EquipmentSlotType.Neck,
        EquipmentSlotType.Ring,
    ];

    public async Task<IReadOnlyCollection<EquipmentItemViewModel>> GetEquipmentAsync()
    {
        var items = await persistenceService.GetEquipmentAsync();
        items = items.OrderBy(ei => ei.EquipmentSlotType).ThenByDescending(ei => ei.EquipmentRarity)
            .ThenByDescending(ei => ei.Level).ToList();
        return mapper.Map<IReadOnlyCollection<EquipmentItemViewModel>>(items);
    }

    public async Task<IReadOnlyCollection<EquipmentSlotTypeViewModel>> GetEquipmentSlotTypesAsync()
    {
        var equipmentData = await coreDataCache.GetEquipmentDataAsync();

        return SlotTypes.Select(x =>
        {
            if (!equipmentData.SlotTypeNames.TryGetValue(x, out var name))
            {
                name = x.ToString();
            }

            return new EquipmentSlotTypeViewModel
            {
                SlotType = x,
                Name = name,
                IconUrl = equipmentSlotTypeIconUrlProvider.GetIconUrl(x),
            };
        }).ToList();
    }

    public async Task<IReadOnlyCollection<EquipmentInsightsViewModel>> GetEquipmentInsights(string unitId,
        CancellationToken ct)
    {
        var equipmentData = await coreDataCache.GetEquipmentDataAsync();
        var dtos = await equipmentService.GetInsightsAsync(unitId, ct);
        return dtos.OrderBy(x => x.FromLevel).Select(dto =>
                equipmentInsightsViewModelFactory.Create(dto, equipmentData.StatAttributeNames, equipmentData.SetNames))
            .ToList();
    }
}

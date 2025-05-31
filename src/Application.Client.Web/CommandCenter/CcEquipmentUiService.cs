using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter;

public class CcEquipmentUiService(IPersistenceService persistenceService, IMapper mapper) : ICcEquipmentUiService
{
    public async Task<IReadOnlyCollection<EquipmentItemViewModel>> GetEquipmentAsync()
    {
        var items = await persistenceService.GetEquipmentAsync();
        items = items.OrderBy(ei => ei.EquipmentSlotType).ThenByDescending(ei => ei.EquipmentRarity)
            .ThenByDescending(ei => ei.Level).ToList();
        return mapper.Map<IReadOnlyCollection<EquipmentItemViewModel>>(items);
    }
}
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class EquipmentSlotTypeIconUrlProvider(IAssetUrlProvider assetUrlProvider) : IEquipmentSlotTypeIconUrlProvider
{
    public string GetIconUrl(EquipmentSlotType equipmentSlotType)
    {
        var id = equipmentSlotType switch
        {
            EquipmentSlotType.Garment => "icon_flat_equipment_slot_armour",
            EquipmentSlotType.Hand => "icon_flat_equipment_slot_armour_weapon",
            _ => "",
        };

        return assetUrlProvider.GetHohIconUrl(id);
    }
}
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class EquipmentSlotTypeIconUrlProvider(IAssetUrlProvider assetUrlProvider) : IEquipmentSlotTypeIconUrlProvider
{
    public string GetIconUrl(EquipmentSlotType equipmentSlotType)
    {
        var id = equipmentSlotType switch
        {
            EquipmentSlotType.Garment => "icon_flat_equipment_slot_armour_neutral",
            EquipmentSlotType.Hand => "icon_flat_equipment_slot_armour_weapon_neutral",
            EquipmentSlotType.Undefined => "icon_flat_equipment_slot_armour_weapon_neutral",
            EquipmentSlotType.Ring => "icon_flat_equipment_slot_armour_ring_neutral",
            EquipmentSlotType.Hat => "icon_flat_equipment_slot_hat_neutral",
            EquipmentSlotType.Neck => "icon_flat_equipment_slot_neck_neutral",
            _ => "",
        };

        return assetUrlProvider.GetHohIconUrl(id);
    }
}

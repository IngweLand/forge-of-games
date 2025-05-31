using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Extensions;

public static class EquipmentExtensions
{
    public static int ToStarCount(this EquipmentRarity rarity)
    {
        return rarity switch
        {
            EquipmentRarity.Star_3 => 3,
            EquipmentRarity.Star_4 => 4,
            EquipmentRarity.Star_5 => 5,
            _ => 0,
        };
    }
}
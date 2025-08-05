using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Extensions;

public static class RelicExtensions
{
    public static int ToStarCount(this RelicRarity rarity)
    {
        return rarity switch
        {
            RelicRarity.Star_4 => 4,
            RelicRarity.Star_5 => 5,
            _ => 0,
        };
    }
}

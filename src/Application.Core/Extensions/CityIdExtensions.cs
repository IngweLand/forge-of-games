using Ingweland.Fog.Models.Hoh.Constants;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Extensions;

public static class CityIdExtensions
{
    public static IReadOnlyCollection<WonderId> GetWonders(this CityId cityId)
    {
        return cityId switch
        {
            CityId.Mayas_Tikal => GetMayasWonders(),
            CityId.Mayas_ChichenItza => GetMayasWonders(),
            CityId.Mayas_SayilPalace => GetMayasWonders(),
            CityId.China => GetChineseWonders(),
            _ => Array.Empty<WonderId>()
        };
    }

    public static string ToDefaultAge(this CityId cityId)
    {
        return cityId switch
        {
            CityId.Mayas_Tikal => AgeIds.MAYAS,
            CityId.Mayas_ChichenItza => AgeIds.MAYAS,
            CityId.Mayas_SayilPalace => AgeIds.MAYAS,
            CityId.China => AgeIds.CHINA,
            CityId.Vikings => AgeIds.VIKINGS,
            CityId.Egypt => AgeIds.EGYPT,
            _ => AgeIds.BRONZE_AGE
        };
    }

    private static List<WonderId> GetMayasWonders()
    {
        return [WonderId.Mayas_Tikal, WonderId.Mayas_ChichenItza, WonderId.Mayas_SayilPalace];
    }

    private static List<WonderId> GetChineseWonders()
    {
        return [WonderId.China_ForbiddenCity, WonderId.China_GreatWall, WonderId.China_TerracottaArmy];
    }
}
using Ingweland.Fog.Models.Hoh.Constants;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Extensions;

public static class CityIdExtensions
{
    private static readonly List<WonderId> MayaWonders =
        [WonderId.Mayas_Tikal, WonderId.Mayas_ChichenItza, WonderId.Mayas_SayilPalace];

    private static readonly List<WonderId> ChineseWonders =
        [WonderId.China_ForbiddenCity, WonderId.China_GreatWall, WonderId.China_TerracottaArmy];

    private static readonly List<WonderId> EgyptianWonders =
        [WonderId.Egypt_AbuSimbel, WonderId.Egypt_CheopsPyramid, WonderId.Egypt_GreatSphinx];

    private static readonly List<WonderId> VikingsWonders =
        [WonderId.Vikings_Valhalla, WonderId.Vikings_Yggdrasil, WonderId.Vikings_DragonshipEllida];

    public static IReadOnlyCollection<WonderId> GetWonders(this CityId cityId)
    {
        return cityId switch
        {
            CityId.Mayas_Tikal => MayaWonders,
            CityId.Mayas_ChichenItza => MayaWonders,
            CityId.Mayas_SayilPalace => MayaWonders,
            CityId.China => ChineseWonders,
            CityId.Egypt => EgyptianWonders,
            CityId.Vikings => VikingsWonders,
            _ => Array.Empty<WonderId>(),
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
            _ => AgeIds.BRONZE_AGE,
        };
    }

    public static CityId ToDefaultTechnologyCity(this CityId cityId)
    {
        return cityId is CityId.Mayas_Tikal or CityId.Mayas_ChichenItza or CityId.Mayas_SayilPalace
            ? CityId.Mayas_ChichenItza
            : cityId;
    }

    public static string GetIcon(this CityId cityId)
    {
        return cityId switch
        {
            CityId.Mayas_Tikal => "icon_city_crest_maya",
            CityId.Mayas_ChichenItza => "icon_city_crest_maya",
            CityId.Mayas_SayilPalace => "icon_city_crest_maya",
            CityId.China => "icon_city_crest_china",
            CityId.Vikings => "icon_city_crest_vikings",
            CityId.Egypt => "icon_city_crest_egypt",
            _ => string.Empty,
        };
    }

    public static string ToInGameId(this CityId cityId)
    {
        return $"city.City_{cityId}";
    }
}

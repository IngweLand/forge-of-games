using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.HohCoreDataParserSdk.Extensions;

public static class WorldTypeExtensions
{
    public static WorldType ToWorldType(this string src)
    {
        return src switch
        {
            "world_type.tesla_storm" => WorldType.TeslaStorm,
            "world_type.campaign" => WorldType.Campaign,
            "world_type.historic_battle" => WorldType.HistoricBattle,
            _ => throw new Exception($"Cannot map world type: {src}"),
        };
    }
}

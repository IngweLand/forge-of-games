using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Extensions;

public static class WorldTypeExtensions
{
    public static WorldType ToWorldType(this string src)
    {
        return src switch
        {
            "world_type.tesla_storm" => WorldType.TeslaStorm,
            "world_type.campaign" => WorldType.Campaign,
            _ => throw new Exception($"Cannot map world type: {src}"),
        };
    }
}

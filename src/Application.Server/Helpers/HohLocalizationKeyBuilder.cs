using Ingweland.Fog.Application.Server.Enums.Hoh;

namespace Ingweland.Fog.Application.Server.Helpers;

public static class HohLocalizationKeyBuilder
{
    public static string BuildKey(HohLocalizationCategory category, HohLocalizationProperty property, string id)
    {
        return $"Base.{category}.{id}_{property}";
    }

    public static string BuildKey(HohLocalizationCategory category, string id)
    {
        return $"Base.{category}.{id}";
    }
}

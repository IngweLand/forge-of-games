using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.HohCoreDataParserSdk.Extensions;

public static class UnitColorExtensions
{
    public static UnitColor ToUnitColor(this string value)
    {
        return value.ToLower() switch
        {
            "blue" => UnitColor.Blue,
            "green" => UnitColor.Green,
            "neutral" => UnitColor.Neutral,
            "purple" => UnitColor.Purple,
            "red" => UnitColor.Red,
            "yellow" => UnitColor.Yellow,
            _ => throw new Exception($"Cannot map unit color: {value}"),
        };
    }
}

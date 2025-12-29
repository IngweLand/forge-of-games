using System.Globalization;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Extensions;

public static class NumericValueExtensions
{
    private const int DEFAULT_HITS_PER_MINUTE = 60;

    public static string ToFormatedString(this BattleAbilityDescriptionItemValue src)
    {
        switch (src.Type)
        {
            case NumericValueType.Duration:
                return $"{src.Value} s";
            case NumericValueType.Percentage:
            {
                var rounded = (float) Math.Round(src.Value * 100, 2);
                var result = rounded % 1 == 0 ? rounded.ToString("0") : rounded.ToString("0.00");
                return $"{result}%";
            }
            case NumericValueType.Number:
                return src.Value.ToString(CultureInfo.InvariantCulture);
            default:
                throw new ArgumentOutOfRangeException(src.Type.ToString());
        }
    }
    
    public static string ToFormatedString(this NumericValueType src, float value)
    {
        return src switch
        {
            NumericValueType.Duration => $"{value}s",
            NumericValueType.Percentage => (value * 100).ToString("N1"),
            NumericValueType.Speed => Math.Round(DEFAULT_HITS_PER_MINUTE * value, MidpointRounding.AwayFromZero)
                .ToString(CultureInfo.InvariantCulture),
            _ => value.ToString(CultureInfo.InvariantCulture),
        };
    }
    
    public static string ToFormatedString2(this NumericValueType src, float value)
    {
        return src switch
        {
            NumericValueType.Duration => $"{Math.Round(value, 2, MidpointRounding.AwayFromZero)} s",
            NumericValueType.Percentage => $"{Math.Round(value * 100, 2)} %",
            NumericValueType.Speed => Math.Round(DEFAULT_HITS_PER_MINUTE * value, MidpointRounding.AwayFromZero)
                .ToString(CultureInfo.InvariantCulture),
            _ => Math.Round(value, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture),
        };
    }
}

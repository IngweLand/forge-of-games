using System.Globalization;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Extensions;

public static class NumericValueExtensions
{
    public static string ToFormatedString(this HeroAbilityDescriptionItemValue src)
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
}

using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;

namespace Ingweland.Fog.HohCoreDataParserSdk.Converters;

public class
    BuildingLevelDynamicChangeDtoConverter : ITypeConverter<BuildingLevelDynamicChangeDTO, Dictionary<int, float>>
{
    public Dictionary<int, float> Convert(BuildingLevelDynamicChangeDTO source, Dictionary<int, float> destination,
        ResolutionContext context)
    {
        var values = new Dictionary<int, float>();
        for (var i = 0; i < source.Values.Count - 1; i++)
        {
            var current = int.Parse(source.Values[i].When);
            var next = int.Parse(source.Values[i + 1].When);
            var currentValue = source.Values[i].PackedThen.Unpack<DynamicFloatValueDTO>().Value;
            while (current < next)
            {
                values.Add(current, currentValue);
                current++;
            }
        }

        var lastLevel = int.Parse(source.Values.Last().When);
        var lastValue = source.Values.Last().PackedThen.Unpack<DynamicFloatValueDTO>().Value;
        values.Add(lastLevel, lastValue);

        return values;
    }
}
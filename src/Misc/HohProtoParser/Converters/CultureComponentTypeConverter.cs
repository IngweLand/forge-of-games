using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Shared.Helpers;

namespace HohProtoParser.Converters;

public class CultureComponentTypeConverter : ITypeConverter<CultureComponentDTO, CultureComponent>
{
    public CultureComponent Convert(CultureComponentDTO source, CultureComponent destination, ResolutionContext context)
    {
        var dynamicDefinitions =
            (IList<DynamicFloatValueDefinitionDTO>) context.Items[ContextKeys.DYNAMIC_FLOAT_VALUE_DEFINITIONS];
        var ages = (IDictionary<string, Age>) context.Items[ContextKeys.AGES];
        var ranges = new Dictionary<int, int>();
        if (!string.IsNullOrWhiteSpace(source.DynamicRangesId))
        {
            var rangeDefinitions = dynamicDefinitions.First(src => src.Id == source.DynamicRangesId);
            ProcessValues(rangeDefinitions, ranges);
        }

        var values = new Dictionary<string, Dictionary<int, int>>();
        if (!string.IsNullOrWhiteSpace(source.DynamicRangesId))
        {
            var valueDefinitions = dynamicDefinitions.First(src => src.Id == source.DynamicValuesId);
            var ageValues = valueDefinitions.PackedMapping.Unpack<BuildingAgeDynamicChangeDTO>().Values;
            foreach (var ageValue in ageValues)
            {
                var ageLevelValues = new Dictionary<int, int>();
                var ageLevelDefinitionId = ageValue.PackedThen.Unpack<DynamicFloatValueDTO>().DynamicValueId;
                var ageLevelDefinitions = dynamicDefinitions.First(src => src.Id == ageLevelDefinitionId);
                ProcessValues(ageLevelDefinitions, ageLevelValues);
                values.Add(HohStringParser.GetConcreteId(ageValue.When), ageLevelValues);
            }

            var importantAges = ages.Where(kvp => kvp.Value.Index is > 1 and < 30 && kvp.Value.Id != "ComingSoon")
                .Select(kvp => kvp.Value).OrderBy(x => x.Index).ToList();
            foreach (var age in importantAges)
            {
                if (values.ContainsKey(age.Id))
                {
                    continue;
                }

                var previousAge = importantAges.First(x => x.Index == age.Index - 1);
                var previousAgeValues = values[previousAge.Id];
                values.Add(age.Id, previousAgeValues);
            }
        }

        return new CultureComponent
        {
            Range = source.HasRange ? source.Range : null,
            Value = source.HasValue ? source.Value : null,
            Ranges = ranges,
            Values = values,
        };
    }

    private static void ProcessValues(DynamicFloatValueDefinitionDTO rangeDefinitions, Dictionary<int, int> ranges)
    {
        var levels = rangeDefinitions.PackedMapping.Unpack<BuildingLevelDynamicChangeDTO>();

        for (var i = 0; i < levels.Values.Count - 1; i++)
        {
            var current = int.Parse(levels.Values[i].When);
            var next = int.Parse(levels.Values[i + 1].When);
            var currentValue = levels.Values[i].PackedThen.Unpack<DynamicFloatValueDTO>().Value;
            while (current < next)
            {
                ranges.Add(current, (int) currentValue);
                current++;
            }
        }

        var lastLevel = int.Parse(levels.Values.Last().When);
        var lastValue = levels.Values.Last().PackedThen.Unpack<DynamicFloatValueDTO>().Value;
        ranges.Add(lastLevel, (int) lastValue);
    }
}

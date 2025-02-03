using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class CultureComponent : ComponentBase
{
    [ProtoMember(1)]
    public int? Range { get; init; }

    [ProtoMember(2)]
    public IReadOnlyDictionary<int, int> Ranges { get; init; } = new Dictionary<int, int>();

    [ProtoMember(3)]
    public int? Value { get; init; }

    [ProtoMember(4)]
    public IReadOnlyDictionary<string, Dictionary<int, int>> Values { get; init; } =
        new Dictionary<string, Dictionary<int, int>>();

    public int GetValue(string ageId, int level)
    {
        if (Value.HasValue)
        {
            return Value.Value;
        }

        if (!Values.TryGetValue(ageId, out var ageValues))
        {
            return 0;
        }
        
        if (ageValues.TryGetValue(level, out var value))
        {
            return value;
        }

        if (level > ageValues.Last().Key)
        {
            return ageValues.Last().Value;
        }

        throw new ArgumentException($"Could not find happiness value for level {level}");
    }

    public int GetRange(int level)
    {
        if (Range.HasValue)
        {
            return Range.Value;
        }
        else
        {
            if (Ranges.TryGetValue(level, out var range))
            {
                return range;
            }

            if (level > Ranges.Last().Key)
            {
                return Ranges.Last().Value;
            }

            throw new ArgumentException($"Could not find range value for level - {level}");
        }
    }
}

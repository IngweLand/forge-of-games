using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class BoostResourceComponent : ComponentBase
{
    [ProtoMember(1)]
    public HashSet<CityId> CityIds { get; init; } = [];

    [ProtoMember(2)]
    public string? ResourceId { get; init; }
    [ProtoMember(3)]
    public ResourceType ResourceType { get; init; }

    [ProtoMember(4)]
    public double? Value { get; init; }
    
    [ProtoMember(5)]
    public IReadOnlyDictionary<int, double> Values { get; init; } = new Dictionary<int, double>();

    public double GetValue(int level = 0)
    {
        if (Values.Count > 0)
        {
            if (Values.TryGetValue(level, out var value))
            {
                return value;
            }

            if (level > Values.Last().Key)
            {
                return Values.Last().Value;
            }

            throw new ArgumentException($"Could not find value for level - {level}");
        }
        else  if (Value.HasValue)
        {
            return Value.Value;
        }

        return 0;
    }
}

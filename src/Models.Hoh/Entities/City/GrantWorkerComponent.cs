using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class GrantWorkerComponent : ComponentBase
{
    [ProtoMember(1)]
    public int? WorkerCount { get; init; }

    [ProtoMember(2)]
    public IReadOnlyDictionary<int, int> WorkerCounts { get; init; } = new Dictionary<int, int>();

    [ProtoMember(3)]
    public WorkerType WorkerType { get; init; }

    public int GetWorkerCount(int level = 0)
    {
        if (WorkerCount.HasValue)
        {
            return WorkerCount.Value;
        }
        else
        {
            if (WorkerCounts.TryGetValue(level, out var value))
            {
                return value;
            }

            if (level > WorkerCounts.Last().Key)
            {
                return WorkerCounts.Last().Value;
            }

            throw new ArgumentException($"Could not find worker count for level - {level}");
        }
    }
}
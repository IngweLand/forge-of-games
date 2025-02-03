using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class ConstructionComponent : ComponentBase
{
    [ProtoMember(1)]
    public int BuildTime { get; init; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<ResourceAmount> Cost { get; init; }

    [ProtoMember(3)]
    public int WorkerCount { get; init; }
}

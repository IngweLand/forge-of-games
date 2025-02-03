using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class ProductionComponent : ComponentBase
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<ResourceAmount> Cost { get; init; } = new List<ResourceAmount>();

    [ProtoMember(2)]
    public double Factor { get; init; }

    [ProtoMember(3)]
    public int ProductionTime { get; init; }

    [ProtoMember(4)]
    public required IReadOnlyCollection<RewardBase> Products { get; init; } = new List<RewardBase>();

    [ProtoMember(5)]
    public int WorkerCount { get; init; }

    [ProtoMember(6)]
    public required string Id { get; init; }
}

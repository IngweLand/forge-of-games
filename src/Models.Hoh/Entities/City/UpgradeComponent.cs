using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class UpgradeComponent : ComponentBase
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<ResourceAmount> Cost { get; init; }

    [ProtoMember(2)]
    public required string NextBuildingId { get; init; }

    [ProtoMember(3)]
    public int UpgradeTime { get; init; }

    [ProtoMember(4)]
    public int WorkerCount { get; init; }
}

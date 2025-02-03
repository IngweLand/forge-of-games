using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class GrantWorkerComponent : ComponentBase
{
    [ProtoMember(1)]
    public int WorkerCount { get; init; }
}

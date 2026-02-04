using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class WorkerBehaviour
{
    [ProtoMember(1)]
    public int Amount { get; set; }

    [ProtoMember(2)]
    public WorkerType Type { get; set; }
}

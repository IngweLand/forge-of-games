using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class WonderCrate
{
    [ProtoMember(1)]
    public required int Amount { get; init; }
    [ProtoMember(2)]
    public required ResourceAmount FillResource { get; init; }
}

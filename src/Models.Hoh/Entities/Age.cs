using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities;

[ProtoContract]
public class Age
{
    [ProtoMember(1)]
    public required string Id { get; init; }

    [ProtoMember(2)]
    public int Index { get; init; }

    public override string ToString()
    {
        return Id;
    }
}

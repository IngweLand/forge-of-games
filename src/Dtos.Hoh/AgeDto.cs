using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh;

[ProtoContract]
public class AgeDto
{
    [ProtoMember(1)]
    public required string Id { get; init; }

    [ProtoMember(2)]
    public required int Index { get; init; }

    [ProtoMember(3)]
    public required string Name { get; init; }
}

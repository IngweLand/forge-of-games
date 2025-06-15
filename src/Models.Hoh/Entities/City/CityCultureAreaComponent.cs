using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class CityCultureAreaComponent:ComponentBase
{
    [ProtoMember(1)]
    public int Height { get; init; }

    [ProtoMember(2)]
    public required string Id { get; init; }

    [ProtoMember(3)]
    public int Value { get; init; }

    [ProtoMember(4)]
    public int Width { get; init; }

    [ProtoMember(5)]
    public int X { get; init; }

    [ProtoMember(6)]
    public int Y { get; init; }
}
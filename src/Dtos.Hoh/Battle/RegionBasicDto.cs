using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

[ProtoContract]
public class RegionBasicDto
{
    [ProtoMember(1)]
    public required RegionId Id { get; init; }

    [ProtoMember(2)]
    public required string Name { get; init; }
}
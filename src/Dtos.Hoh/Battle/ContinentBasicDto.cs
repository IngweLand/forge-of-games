using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;
[ProtoContract]
public class ContinentBasicDto
{
    [ProtoMember(1)]
    public required ContinentId Id { get; init; }
    [ProtoMember(2)]
    public required string Name { get; init; }
    [ProtoMember(3)]
    public required IReadOnlyCollection<RegionBasicDto> Regions { get; init; }
}
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class Expansion : ExpansionBasicData
{
    [ProtoMember(1)]
    public string? LinkedExpansionId { get; init; }
    [ProtoMember(2)]
    public ExpansionSubType SubType { get; init; }
    [ProtoMember(3)]
    public ExpansionType Type { get; init; }
}

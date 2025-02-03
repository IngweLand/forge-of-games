using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class InitialGridComponent
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<ExpansionBasicData> Expansions { get; init; }
    [ProtoMember(2)]
    public required int ExpansionSize { get; init; }
}

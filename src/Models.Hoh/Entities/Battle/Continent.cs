using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class Continent
{
    [ProtoMember(1)]
    public ContinentId Id { get; init; }

    [ProtoMember(2)]
    public int Index { get; init; }

    [ProtoMember(3)]
    public required IReadOnlyCollection<Region> Regions { get; init; }
}

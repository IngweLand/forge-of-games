using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class World
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<Continent> Continents { get; init; }

    [ProtoMember(2)]
    public WorldId Id { get; init; }

    [ProtoMember(3)]
    public WorldType Type { get; init; }
}

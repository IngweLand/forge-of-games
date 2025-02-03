using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class WonderLevelCost
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<WonderCrate> Crates { get; init; } = new List<WonderCrate>();
    [ProtoMember(2)]
    public required int Level { get; init; }
}

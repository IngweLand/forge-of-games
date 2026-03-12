using ProtoBuf;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.Research;

[ProtoContract]
[ProtoReserved(2, "Was used for CityId property, which was discarded in the game.")]
public class Technology
{
    [ProtoMember(1)]
    public required Age Age { get; init; }

    [ProtoMember(3)]
    public required int HorizontalIndex { get; init; }

    [ProtoMember(4)]
    public required string Id { get; init; }

    [ProtoMember(5)]
    public required string Name { get; init; }

    [ProtoMember(6)]
    public required ResearchComponent ResearchComponent { get; init; }

    [ProtoMember(7)]
    public required int VerticalIndex { get; init; }
    
    [ProtoMember(8)]
    public required HashSet<CityId> CityIds { get; init; }
}

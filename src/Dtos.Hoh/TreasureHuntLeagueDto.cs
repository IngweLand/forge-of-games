using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh;

[ProtoContract]
public class TreasureHuntLeagueDto
{
    [ProtoMember(1)]
    public required TreasureHuntLeague League { get; init; }
    [ProtoMember(2)]
    public required string Name { get; init; }
}

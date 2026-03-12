using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

[ProtoContract]
public class BattleEventBasicDto
{
    [ProtoMember(1)]
    public required int EncounterCount { get; init; }

    [ProtoMember(2)]
    public required int EncounterStartIndex { get; init; }

    [ProtoMember(3)]
    public required RegionId Id { get; init; }

    [ProtoMember(4)]
    public required string Name { get; init; }
}

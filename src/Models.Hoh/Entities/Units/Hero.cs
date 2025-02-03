using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class Hero
{
    [ProtoMember(1)]
    public required string Id { get; init; }

    [ProtoMember(2)]
    public required string UnitId { get; init; }
    
    [ProtoMember(3)]
    public required UnitType SupportUnitType { get; init; }
    [ProtoMember(4)]
    public required HeroClassId ClassId { get; init; }
    [ProtoMember(5)]
    public required string AwakeningId { get; init; }
    [ProtoMember(6)]
    public required string AbilityId { get; init; }
    [ProtoMember(7)]
    public required HeroProgressionComponent ProgressionComponent { get; init; }
    
}

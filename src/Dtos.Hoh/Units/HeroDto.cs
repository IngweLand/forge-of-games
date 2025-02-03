using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class HeroDto
{
    [ProtoMember(1)]
    public required IReadOnlyDictionary<int, IReadOnlyCollection<ResourceAmount>> AscensionCosts { get; init; }

    [ProtoMember(2)]
    public required string Id { get; set; }

    [ProtoMember(3)]
    public string? Info { get; init; }

    [ProtoMember(4)]
    public required IReadOnlyCollection<HeroProgressionCostResource> ProgressionCosts { get; init; }

    [ProtoMember(5)]
    public required HeroStarClass StarClass { get; init; }

    [ProtoMember(6)]
    public required UnitDto Unit { get; init; }
    
    [ProtoMember(7)]
    public required UnitDto BaseSupportUnit { get; init; }

    [ProtoMember(8)]
    public required HeroAwakeningComponent AwakeningComponent { get; init; }

    [ProtoMember(9)]
    public required HeroAbilityDto Ability { get; set; }
    [ProtoMember(10)]
    public required HeroClassId ClassId { get; init; }
    [ProtoMember(11)]
    public required string ClassName { get; init; }
}
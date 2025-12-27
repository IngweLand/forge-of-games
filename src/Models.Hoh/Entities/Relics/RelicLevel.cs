using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Relics;

[ProtoContract]
public class RelicLevel
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<string> Abilities { get; init; } = new List<string>();
    
    [ProtoMember(2)]
    public bool Ascension { get; init; }
    
    [ProtoMember(3)]
    public int AscensionLevel { get; init; }
    
    [ProtoMember(4)]
    public required int Level { get; init; }
    
    [ProtoMember(5)]
    public required string RelicCostDefinitionId { get; init; }
    
    [ProtoMember(6)]
    public required IReadOnlyCollection<RelicStatBoost> Boosts { get; init; } = new List<RelicStatBoost>();
}

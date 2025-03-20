using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class Encounter
{
    [ProtoMember(1)]
    public required IReadOnlyDictionary<Difficulty, EncounterDetails> Details { get; init; }

    [ProtoMember(2)]
    public required string Id { get; init; }

    [ProtoMember(3)]
    public int Index { get; set; }
}
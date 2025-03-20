using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

[ProtoContract]
public class EncounterDto
{
    [ProtoMember(1)]
    public required string Id { get; init; }

    [ProtoMember(2)]
    public IReadOnlyDictionary<Difficulty, EncounterDetailsDto> Details { get; init; } =
        new Dictionary<Difficulty, EncounterDetailsDto>();
}
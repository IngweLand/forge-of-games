using System.Text.Json.Serialization;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class ExpansionCosts
{
    [ProtoMember(1)]
    public required CityId CityId { get; init; }

    [ProtoMember(2)]
    public required IList<ComponentBase> Components { get; init; }

    [ProtoIgnore]
    [JsonIgnore]
    public IReadOnlyCollection<ConstructionComponent> ConstructionComponents =>
        Components.OfType<ConstructionComponent>().ToList();

    [ProtoMember(3)]
    public string? ExpansionId { get; init; }

    [ProtoMember(4)]
    public required string Id { get; init; }

    [ProtoMember(5)]
    public required ExpansionUnlockingType UnlockingType { get; init; }
}

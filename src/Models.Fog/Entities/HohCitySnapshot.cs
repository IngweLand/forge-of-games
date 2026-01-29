using System.Text.Json.Serialization;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public class HohCitySnapshot
{
    [JsonIgnore]
    [ProtoIgnore]
    public string ComputedName => Name ?? CreatedDateUtc.ToLocalTime().ToString("G");

    [ProtoMember(1)]
    public required DateTime CreatedDateUtc { get; init; }
    [ProtoMember(2)]
    public IReadOnlyCollection<HohCityMapEntity> Entities { get; init; } = new List<HohCityMapEntity>();
    [ProtoMember(3)]
    public required string Id { get; init; }
    [ProtoMember(4)]
    public string? Name { get; init; }
}

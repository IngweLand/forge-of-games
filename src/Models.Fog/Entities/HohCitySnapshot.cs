using System.Text.Json.Serialization;

namespace Ingweland.Fog.Models.Fog.Entities;

public class HohCitySnapshot
{
    public required DateTime CreatedDateUtc { get; init; }
    public IReadOnlyCollection<HohCityMapEntity> Entities { get; init; } = new List<HohCityMapEntity>();
    public required string Id { get; init; }
    public string? Name { get; init; }

    [JsonIgnore]
    public string ComputedName => Name ?? CreatedDateUtc.ToLocalTime().ToString("G");
}

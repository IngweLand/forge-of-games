using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class HohCity
{
    public required string AgeId { get; set; }
    public IReadOnlyCollection<HohCityMapEntity> Entities { get; set; } = new List<HohCityMapEntity>();
    public required string Id { get; set; }
    public CityId InGameCityId { get; set; }
    public required string Name { get; set; }
    public HashSet<string> UnlockedExpansions { get; set; } = [];
    public IReadOnlyCollection<HohCitySnapshot> Snapshots { get; init; } = new List<HohCitySnapshot>();

    public WonderId WonderId { get; set; }
    public int WonderLevel { get; set; }
}

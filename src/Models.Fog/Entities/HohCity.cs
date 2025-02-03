using Ingweland.Fog.Models.Hoh.Constants;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class HohCity
{
    public required string Id { get; set; }
    public CityId InGameCityId { get; set; }
    public IList<HohCityMapEntity> Entities { get; set; } = new List<HohCityMapEntity>();
    public required string AgeId { get; set; }
    public required string Name { get; set; }
}

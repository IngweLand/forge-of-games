using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;

public class CitySnapshotSearchPreferenceViewModel
{
    public required string Name { get; init; }
    public required CitySnapshotSearchPreference Value { get; set; }
}

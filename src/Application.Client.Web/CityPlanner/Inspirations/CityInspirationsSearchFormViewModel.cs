using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;

public class CityInspirationsSearchFormViewModel
{
    public required IReadOnlyCollection<AgeViewModel> Ages { get; init; }
    public required IReadOnlyCollection<HohCityBasicData> Cities { get; init; }
    public required IReadOnlyCollection<CitySnapshotSearchPreferenceViewModel> SearchPreferences { get; init; }
}

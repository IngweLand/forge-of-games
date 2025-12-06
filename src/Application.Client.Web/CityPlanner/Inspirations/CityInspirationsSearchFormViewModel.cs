using Ingweland.Fog.Application.Client.Web.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;

public class CityInspirationsSearchFormViewModel
{
    public required IReadOnlyCollection<AgeViewModel> Ages { get; init; }
    public required IReadOnlyCollection<HohCityBasicData> Cities { get; init; }
    public required IReadOnlyCollection<LabeledValue<CityProductionMetric>> ProductionMetrics { get; init; }
    public required IReadOnlyCollection<CitySnapshotSearchPreferenceViewModel> SearchPreferences { get; init; }
}

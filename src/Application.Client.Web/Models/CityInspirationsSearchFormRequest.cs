using Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;
using Ingweland.Fog.Application.Client.Web.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Application.Client.Web.Models;

public class CityInspirationsSearchFormRequest
{
    public AgeViewModel? Age { get; set; }
    public bool AllowPremiumCultureBuildings { get; set; }
    public bool AllowPremiumFarmBuildings { get; set; }
    public bool AllowPremiumHomeBuildings { get; set; }
    public HohCityBasicData? City { get; set; }
    public LabeledValue<CityProductionMetric>? ProductionMetric { get; set; }
    public CitySnapshotSearchPreferenceViewModel? SearchPreference { get; set; }
}

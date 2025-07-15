using Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Models;

public class CityInspirationsSearchFormRequest
{
    public AgeViewModel? Age { get; set; }
    public HohCityBasicData? City { get; set; }
    public CitySnapshotSearchPreferenceViewModel? SearchPreference { get; set; }
    public bool AllowPremium { get; set; }
}

using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class CitiesStatsPage : FogPageBase
{
    private IReadOnlyCollection<CityPlannerCityPropertiesViewModel>? _citiesStats;

    [Inject]
    public ICityPlanner CityPlanner { get; set; }

    [Inject]
    private IPersistenceService PersistenceService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        var cities = await PersistenceService.GetTempCities();
        var citiesStats = new List<CityPlannerCityPropertiesViewModel>();
        foreach (var city in cities.OrderBy(c => c.InGameCityId))
        {
            await CityPlanner.InitializeAsync(city);
            if (CityPlanner.CityMapState.CityPropertiesViewModel != null)
            {
                citiesStats.Add(CityPlanner.CityMapState.CityPropertiesViewModel);
            }
        }

        _citiesStats = citiesStats;
    }
}
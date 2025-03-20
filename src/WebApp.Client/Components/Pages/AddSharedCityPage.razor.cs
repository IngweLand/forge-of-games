using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class AddSharedCityPage : FogPageBase
{
    [Inject]
    private ICityPlannerSharingService CityPlannerSharingService { get; set; }

    [Inject]
    protected IDialogService DialogService { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    private IPersistenceService PersistenceService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }
        
        HohCity? city;
        try
        {
            city = await CityPlannerSharingService.GetSharedCityAsync(SharedCityId!);
        }
        catch (Exception e)
        {
            OpenMainPage();
            return;
        }

        if (city != null)
        {
            await CreateNewCity(city);
        }
        else
        {
            OpenMainPage();
        }
    }

    private async Task CreateNewCity(HohCity city)
    {
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            BackgroundClass = "dialog-blur-bg",
            Position = DialogPosition.TopCenter
        };
        var dialog = await DialogService.ShowAsync<CreateNewCityDialog>(null, options);
        var result = await dialog.Result;
        if (result == null || result.Canceled)
        {
            OpenMainPage();
            return;
        }

        var cityName = result.Data as string;
        if (string.IsNullOrWhiteSpace(cityName))
        {
            OpenMainPage();
            return;
        }

        city.Id = Guid.NewGuid().ToString("N");
        city.Name = cityName;
        await PersistenceService.SaveCity(city);

        OpenMainPage();
    }

    private void OpenMainPage()
    {
        NavigationManager.NavigateTo("city-planner", false, true);
    }
}

using System.Web;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class CityPlannerDataConverterPage : FogPageBase
{
    private const string URL_FORMAT_ERROR_MESSAGE = "You must provide a valid url with 'sharedLayout' query value.";

    private string? _cityName;
    private string? _cityPlannerUrl;
    private bool _isConverting;
    private bool _success;

    [Inject]
    public ICityPlannerDataConverterService ConverterService { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [Inject]
    private ILogger<CityPlannerDataConverterPage> Logger { get; set; }

    private IEnumerable<string> ValidateUrl(string src)
    {
        if (string.IsNullOrWhiteSpace(src))
        {
            yield return URL_FORMAT_ERROR_MESSAGE;
        }

        var hasError = false;
        try
        {
            var url = new Uri(src);
            var queryParams = HttpUtility.ParseQueryString(url.Query);
            var sharedLayout = queryParams["sharedLayout"];
            if (string.IsNullOrWhiteSpace(sharedLayout))
            {
                hasError = true;
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error validating url.");
            hasError = true;
        }

        if (hasError)
        {
            yield return URL_FORMAT_ERROR_MESSAGE;
        }
    }

    private async Task Convert()
    {
        _isConverting = true;
        StateHasChanged();

        try
        {
            var url = new Uri(_cityPlannerUrl!);
            var queryParams = HttpUtility.ParseQueryString(url.Query);
            var sharedLayout = queryParams["sharedLayout"];
            await ConverterService.ConvertAsync(sharedLayout!, _cityName!);
            _ = await DialogService.ShowMessageBox(null, $"Successfully created city {_cityName}",
                Loc[FogResource.Common_Ok]);
        }
        catch (Exception e)
        {
            _ = await DialogService.ShowMessageBox("Error converting data", e.Message, Loc[FogResource.Common_Ok]);
        }

        _isConverting = false;
        StateHasChanged();
    }
}

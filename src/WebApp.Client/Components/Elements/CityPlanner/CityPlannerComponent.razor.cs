using System.Diagnostics.CodeAnalysis;
using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Static;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using SkiaSharp.Views.Blazor;
using Size = System.Drawing.Size;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public partial class CityPlannerComponent : ComponentBase, IDisposable
{
    private Size _canvasSize = Size.Empty;
    private bool _fitOnPaint = true;
    private bool _isInitialized;
    private bool _leftPanelIsVisible = true;
    private bool _rightPanelIsVisible = true;
    private SKGLView _skCanvasView;

    [Inject]
    public ICityPlanner CityPlanner { get; set; }

    [Inject]
    public ICityPlannerInteractionManager CityPlannerInteractionManager { get; set; }

    [Inject]
    public CityPlannerSettings CityPlannerSettings { get; set; }

    [Inject]
    public ICityPlannerCommandFactory CommandFactory { get; set; }

    [Inject]
    public ICommandManager CommandManager { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [Inject]
    private IInGameStartupDataService InGameStartupDataService { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    public IPersistenceService PersistenceService { get; set; }

    [Inject]
    public ISnackbar Snackbar { get; set; }

    public void Dispose()
    {
        _skCanvasView?.Dispose();
        Snackbar.Dispose();
        if (_isInitialized)
        {
            CityPlanner.StateHasChanged -= CityPlannerOnStateHasHasChanged;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        CityPlannerSettings.StateChanged += CityPlannerSettingsOnStateChanged;

        var savedCities = await PersistenceService.GetCities();
        var opened = false;
        if (savedCities.Count > 0)
        {
            opened = await OpenCitiesDialog();
        }

        if (!opened)
        {
            await CityPlanner.InitializeAsync();
        }

        CityPlanner.StateHasChanged += CityPlannerOnStateHasHasChanged;
        _isInitialized = true;
    }

    private void BuildingSelectorOnItemClicked(BuildingGroup buildingGroup)
    {
        var cmd = CommandFactory.CreateAddBuildingCommand(buildingGroup);
        CommandManager.ExecuteCommand(cmd);
        _skCanvasView!.Invalidate();
    }

    private void CityPlannerOnStateHasHasChanged()
    {
        _skCanvasView!.Invalidate();
        StateHasChanged();
    }

    private void CityPlannerSettingsOnStateChanged()
    {
        _skCanvasView!.Invalidate();
    }

    private async Task CreateNewCity()
    {
        var options = GetDefaultDialogOptions();
        var dialog = await DialogService.ShowAsync<CreateNewCityDialog>(null, options);
        var result = await dialog.Result;
        if (result == null || result.Canceled)
        {
            return;
        }

        var cityName = result.Data as string;
        if (string.IsNullOrWhiteSpace(cityName))
        {
            return;
        }

        var city = CityPlanner!.CreateNew(cityName);
        await PersistenceService.SaveCity(city);
        await CityPlanner!.InitializeAsync(city);
    }

    private void Delete()
    {
        if (CityPlanner.CityMapState.SelectedCityMapEntity == null)
        {
            return;
        }

        var cmd = CommandFactory.CreateDeleteEntityCommand(CityPlanner.CityMapState.SelectedCityMapEntity);
        CommandManager.ExecuteCommand(cmd);
        _skCanvasView!.Invalidate();
    }

    private void FitToScreen()
    {
        CityPlannerInteractionManager.FitToScreen(_canvasSize);
        _skCanvasView!.Invalidate();
    }

    private static DialogOptions GetDefaultDialogOptions(bool closButton = false)
    {
        return new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            BackgroundClass = "dialog-blur-bg",
            CloseButton = closButton,
        };
    }

    private async Task HandleImportData(string data)
    {
        string? error = null;
        ResourceCreatedResponse? response = null;
        try
        {
            response = await InGameStartupDataService.ImportInGameDataAsync(new ImportInGameStartupDataRequestDto()
            {
                InGameStartupData = data,
            });
        }
        catch (Exception e)
        {
            error = "Could not import data.";
        }

        if (response != null)
        {
            NavigationManager.NavigateTo(response.WebResourceUrl);
        }
        else if (error != null)
        {
            Snackbar!.Add(error, Severity.Error);
        }
    }

    private async Task ImportInGameCity()
    {
        var options = GetDefaultDialogOptions();
        var dialog = await DialogService.ShowAsync<ImportInGameCityDialog>(null, options);
        var result = await dialog.Result;
        if (result == null || result.Canceled)
        {
            return;
        }

        var importedText = result.Data as string;
        if (string.IsNullOrWhiteSpace(importedText))
        {
            return;
        }

        await HandleImportData(importedText);
    }

    private void InteractiveCanvasOnPointerDown(PointerEventArgs args)
    {
        CityPlannerInteractionManager.OnPointerDown((float) args.OffsetX, (float) args.OffsetY);
    }

    private void InteractiveCanvasOnPointerMove(PointerEventArgs args)
    {
        if (args.Buttons != 1)
        {
            return;
        }

        if (CityPlannerInteractionManager.OnPointerMove((float) args.OffsetX, (float) args.OffsetY))
        {
            _skCanvasView!.Invalidate();
        }
    }

    private void InteractiveCanvasOnPointerUp(PointerEventArgs args)
    {
        CityPlannerInteractionManager.OnPointerUp((float) args.OffsetX, (float) args.OffsetY);
    }

    private void InteractiveCanvasOnWheel(WheelEventArgs e)
    {
        if (CityPlannerInteractionManager.Zoom((float) e.OffsetX, (float) e.OffsetY, (float) e.DeltaY))
        {
            _skCanvasView!.Invalidate();
        }
    }

    private async Task OnKeyUp(KeyboardEventArgs args)
    {
        // if (args.CtrlKey)
        // {
        //     switch (args.Code)
        //     {
        //         case "KeyS":
        //         {
        //             await Save();
        //             break;
        //         }
        //         
        //         case "KeyA":
        //         {
        //             SelectGroup();
        //             break;
        //         }
        //     }
        //     
        //     return;
        // }

        switch (args.Code)
        {
            case KeyboardKeys.Delete:
            case KeyboardKeys.Backspace:
            {
                Delete();
                break;
            }

            case "KeyR":
            {
                Rotate();
                break;
            }
        }
    }

    private async Task<bool> OpenCitiesDialog()
    {
        var parameters = new DialogParameters
        {
            {nameof(OpenCityDialog.Cities), await PersistenceService.GetCities()},
        };

        var options = GetDefaultDialogOptions(true);
        var dialog = await DialogService.ShowAsync<OpenCityDialog>(null, parameters, options);
        var result = await dialog.Result;
        if (result == null || result.Canceled)
        {
            return false;
        }

        if (result.Data is not HohCityBasicData cityBasicData)
        {
            return false;
        }

        var city = await PersistenceService.LoadCity(cityBasicData.Id);
        if (city == null)
        {
            return false;
        }

        await CityPlanner!.InitializeAsync(city);
        return true;
    }

    private void Redo()
    {
        CommandManager.Redo();
        _skCanvasView!.Invalidate();
    }

    private void Rotate()
    {
        if (CityPlanner.CityMapState.SelectedCityMapEntity == null)
        {
            return;
        }

        var cmd = CommandFactory.CreateRotateEntityCommand(CityPlanner.CityMapState.SelectedCityMapEntity);
        CommandManager.ExecuteCommand(cmd);
        _skCanvasView!.Invalidate();
    }

    private async Task Save()
    {
        await PersistenceService.SaveCity(CityPlanner!.GetCity());
    }

    private void SelectGroup()
    {
        CityPlanner.SelectGroup();
        _skCanvasView!.Invalidate();
    }

    private void SkCanvasView_OnPaintSurface(SKPaintGLSurfaceEventArgs e)
    {
        var surface = e.Surface;
        var canvas = surface.Canvas;
        _canvasSize = new Size(e.Info.Width, e.Info.Height);
        if (_fitOnPaint)
        {
            CityPlannerInteractionManager.FitToScreen(_canvasSize);
            _fitOnPaint = false;
        }

        CityPlannerInteractionManager.TransformMapArea(canvas);
        CityPlanner.RenderScene(canvas);
    }

    private void ToggleLeftPanel(bool toggled)
    {
        _leftPanelIsVisible = toggled;
    }

    private void ToggleRightPanel(bool toggled)
    {
        _rightPanelIsVisible = toggled;
    }

    private void Undo()
    {
        CommandManager.Undo();
        _skCanvasView!.Invalidate();
    }

    private void ZoomIn()
    {
        if (CityPlannerInteractionManager.Zoom(_canvasSize.Width / 2, _canvasSize.Height / 2, -100))
        {
            _skCanvasView!.Invalidate();
        }
    }

    private void ZoomOut()
    {
        if (CityPlannerInteractionManager.Zoom(_canvasSize.Width / 2, _canvasSize.Height / 2, 100))
        {
            _skCanvasView!.Invalidate();
        }
    }
}

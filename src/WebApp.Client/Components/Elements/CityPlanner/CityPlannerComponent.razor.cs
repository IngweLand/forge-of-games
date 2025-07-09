using System.Diagnostics.CodeAnalysis;
using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SkiaSharp.Views.Blazor;
using Size = System.Drawing.Size;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public partial class CityPlannerComponent : ComponentBase, IDisposable
{
    private Size _canvasSize = Size.Empty;
    private bool _fitOnPaint = true;

    private bool _inventoryIsActive;
    private bool _isInitialized;
    private bool _leftPanelIsVisible = true;
    private bool _rightPanelIsVisible = true;
    private SKGLView _skCanvasView;

    [Inject]
    public ICityPlanner CityPlanner { get; set; }

    [Inject]
    public ICityPlannerInteractionManager CityPlannerInteractionManager { get; set; }

    [Inject]
    private CityPlannerNavigationState CityPlannerNavigationState { get; set; }

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
    private IJSInteropService JsInteropService { get; set; }

    [Inject]
    public IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    public ILocalStorageBackupService LocalStorageBackupService { get; set; }

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

        if (CityPlannerNavigationState.City == null)
        {
            NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.BASE_CITY_PLANNER_PATH, false, true);
            return;
        }

        await LocalStorageBackupService.BackupCities(FogConstants.CITY_PLANNER_VERSION);

        CityPlannerSettings.StateChanged += CityPlannerSettingsOnStateChanged;

        await CityPlanner.InitializeAsync(CityPlannerNavigationState.City);

        CityPlanner.StateHasChanged += CityPlannerOnStateHasHasChanged;
        _isInitialized = true;
    }

    private void BuildingSelectorOnItemClicked(BuildingGroup buildingGroup)
    {
        if (_inventoryIsActive)
        {
            var cmd = CommandFactory.CreateMoveFromInventoryCommand(buildingGroup);
            CommandManager.ExecuteCommand(cmd);
        }
        else
        {
            var cmd = CommandFactory.CreateAddBuildingCommand(buildingGroup);
            CommandManager.ExecuteCommand(cmd);
            _skCanvasView!.Invalidate();
        }
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

    private void Delete()
    {
        if (CityPlanner.CityMapState.SelectedCityMapEntity is not {IsMovable: true})
        {
            return;
        }

        var cmd = CommandFactory.CreateDeleteEntityCommand(CityPlanner.CityMapState.SelectedCityMapEntity);
        CommandManager.ExecuteCommand(cmd);
        _skCanvasView!.Invalidate();
    }

    private void MoveToInventory()
    {
        if (CityPlanner.CityMapState.SelectedCityMapEntities != null)
        {
            CommandManager.ExecuteCommand(CommandFactory.CreateMoveToInventoryCommand(CityPlanner.CityMapState
                .SelectedCityMapEntities.Select(x => x.Id).ToHashSet()));
        }
        else
        {
            if (CityPlanner.CityMapState.SelectedCityMapEntity is not {IsMovable: true})
            {
                return;
            }

            CommandManager.ExecuteCommand(
                CommandFactory.CreateMoveToInventoryCommand(new HashSet<int>([
                    CityPlanner.CityMapState.SelectedCityMapEntity.Id,
                ])));
        }
    }

    private void MoveAllToInventory()
    {
        var cmd = CommandFactory.CreateMoveAllToInventoryCommand();
        CommandManager.ExecuteCommand(cmd);
    }

    private void FitToScreen()
    {
        CityPlannerInteractionManager.FitToScreen(_canvasSize);
        _skCanvasView!.Invalidate();
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
        if (CityPlannerInteractionManager.OnPointerUp((float) args.OffsetX, (float) args.OffsetY))
        {
            _skCanvasView!.Invalidate();
        }
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

    private void Redo()
    {
        CommandManager.Redo();
        _skCanvasView!.Invalidate();
    }

    private void Rotate()
    {
        if (CityPlanner.CityMapState.SelectedCityMapEntity is not {IsMovable: true})
        {
            return;
        }

        var cmd = CommandFactory.CreateRotateEntityCommand(CityPlanner.CityMapState.SelectedCityMapEntity.Id);
        CommandManager.ExecuteCommand(cmd);
        _skCanvasView!.Invalidate();
    }

    private async Task Save()
    {
        await CityPlanner.SaveCityAsync();
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

    private async Task LoadSnapshot(string id)
    {
        await CityPlanner.LoadSnapshot(id);
    }

    private async Task CreateSnapshot()
    {
        await CityPlanner.CreateSnapshot();
    }

    private async Task CompareSnapshots()
    {
        var viewModel = await CityPlanner.CompareSnapshots();

        var parameters = new DialogParameters<SnapshotsComparisonComponent> {{src => src.Data, viewModel}};

        var options = new DialogOptions
        {
            FullWidth = true,
            BackgroundClass = "dialog-blur-bg",
            NoHeader = true,
            CloseOnEscapeKey = true,
        };
        await DialogService.ShowAsync<SnapshotsComparisonComponent>(null, parameters, options);
    }

    private async Task DeleteSnapshot(string id)
    {
        await CityPlanner.DeleteSnapshot(id);
    }

    private void NavigateToDashboard()
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.BASE_CITY_PLANNER_PATH, false, true);
    }

    private async Task PurgeInventory()
    {
        var result = await DialogService.ShowMessageBox(
            null,
            Loc[FogResource.CityPlanner_PurgeInventoryConfirmation],
            Loc[FogResource.Common_Remove], cancelText: Loc[FogResource.Common_Cancel]);
        if (result != null)
        {
            CommandManager.ExecuteCommand(CommandFactory.CreatePurgeInventoryCommand());
        }
    }
}

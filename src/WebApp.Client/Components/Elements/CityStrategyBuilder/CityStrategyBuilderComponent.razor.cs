using System.Diagnostics.CodeAnalysis;
using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityStrategyBuilder;
using Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SkiaSharp.Views.Blazor;
using Size = System.Drawing.Size;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityStrategyBuilder;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public partial class CityStrategyBuilderComponent : ComponentBase, IDisposable
{
    private Size _canvasSize = Size.Empty;
    private bool _fitOnPaint = true;
    private bool _isInitialized;
    private bool _leftPanelIsVisible = true;
    private bool _rightPanelIsVisible = true;
    private SKGLView? _skCanvasView;

    [Inject]
    private ICityPlannerInteractionManager CityPlannerInteractionManager { get; set; }

    [Inject]
    private CityPlannerSettings CityPlannerSettings { get; set; }

    [Inject]
    private ICityStrategyBuilderService CityStrategyBuilderService { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [Inject]
    public IFogSharingUiService FogSharingUiService { get; set; }

    [Inject]
    private IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    private ILogger<CityStrategyBuilderComponent> Logger { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    public void Dispose()
    {
        _skCanvasView?.Dispose();
        CityStrategyBuilderService.StateHasChanged -= CityPlannerOnStateHasHasChanged;
        CityPlannerSettings.StateChanged -= CityPlannerSettingsOnStateChanged;
        CityStrategyBuilderService.Dispose();
        Logger.LogDebug("Disposing CityStrategyComponent");
    }

    protected override async Task OnInitializedAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        await CityStrategyBuilderService.InitializeAsync(Strategy);

        CityPlannerSettings.StateChanged += CityPlannerSettingsOnStateChanged;
        CityStrategyBuilderService.StateHasChanged += CityPlannerOnStateHasHasChanged;
        _isInitialized = true;
    }

    private void BuildingSelectorOnItemClicked(BuildingGroup buildingGroup)
    {
        if (_skCanvasView == null)
        {
            return;
        }

        CityStrategyBuilderService.AddNewCityMapEntity(buildingGroup);
        _skCanvasView.Invalidate();
    }

    private void CityPlannerOnStateHasHasChanged()
    {
        if (_skCanvasView == null)
        {
            return;
        }

        _skCanvasView.Invalidate();
        StateHasChanged();
    }

    private void CityPlannerSettingsOnStateChanged()
    {
        _skCanvasView?.Invalidate();
    }

    private void DeleteCityMapEntity()
    {
        if (_skCanvasView == null)
        {
            return;
        }

        if (CityStrategyBuilderService.DeleteSelectedCityMapEntity())
        {
            _skCanvasView.Invalidate();
        }
    }

    private async Task ShareStrategy()
    {
        await CityStrategyBuilderService.Save();
        var data = FogSharingUiService.CreateSharedData(CityStrategyBuilderService.Strategy);
        var parameters = new DialogParameters<ShareResourceDialog>
        {
            {d => d.Data, data},
            {
                d => d.BaseUrl,
                $"{NavigationManager.BaseUri.TrimEnd('/')}{FogUrlBuilder.PageRoutes.GET_SHARED_STRATEGY_TEMPLATE}"
            },
        };
        _ = await DialogService.ShowAsync<ShareResourceDialog>(null, parameters, GetDefaultDialogOptions());
    }

    private async Task DeleteStrategy()
    {
        await CityStrategyBuilderService.DeleteStrategy();
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITY_STRATEGIES_DASHBOARD_PATH, false, true);
    }

    private Task RenameStrategy(string newName)
    {
        return CityStrategyBuilderService.Rename(newName);
    }

    private void FitToScreen()
    {
        if (_skCanvasView == null)
        {
            return;
        }

        CityPlannerInteractionManager.FitToScreen(_canvasSize);
        _skCanvasView.Invalidate();
    }

    private void InteractiveCanvasOnPointerDown(PointerEventArgs args)
    {
        CityPlannerInteractionManager.OnPointerDown((float) args.OffsetX, (float) args.OffsetY);
    }

    private void InteractiveCanvasOnPointerMove(PointerEventArgs args)
    {
        if (_skCanvasView == null)
        {
            return;
        }

        if (args.Buttons != 1)
        {
            return;
        }

        if (CityPlannerInteractionManager.OnPointerMove((float) args.OffsetX, (float) args.OffsetY))
        {
            _skCanvasView.Invalidate();
        }
    }

    private void InteractiveCanvasOnPointerUp(PointerEventArgs args)
    {
        if (_skCanvasView == null)
        {
            return;
        }

        if (CityPlannerInteractionManager.OnPointerUp((float) args.OffsetX, (float) args.OffsetY))
        {
            _skCanvasView.Invalidate();
        }
    }

    private void InteractiveCanvasOnWheel(WheelEventArgs e)
    {
        if (_skCanvasView == null)
        {
            return;
        }

        if (CityPlannerInteractionManager.Zoom((float) e.OffsetX, (float) e.OffsetY, (float) e.DeltaY))
        {
            _skCanvasView.Invalidate();
        }
    }

    private void OnKeyUp(KeyboardEventArgs args)
    {
        switch (args.Code)
        {
            case KeyboardKeys.Delete:
            case KeyboardKeys.Backspace:
            {
                DeleteCityMapEntity();
                break;
            }

            case "KeyR":
            {
                Rotate();
                break;
            }

            case "KeyD":
            {
                Duplicate();
                break;
            }
        }
    }

    private void Rotate()
    {
        if (_skCanvasView == null)
        {
            return;
        }

        if (CityStrategyBuilderService.RotateSelectedCityMapEntity())
        {
            _skCanvasView.Invalidate();
        }
    }

    private void Duplicate()
    {
        if (_skCanvasView == null)
        {
            return;
        }

        if (CityStrategyBuilderService.DuplicateSelectedCityMapEntity())
        {
            _skCanvasView.Invalidate();
        }
    }

    private Task Save()
    {
        return CityStrategyBuilderService.Save();
    }

    private void RequestSaving()
    {
        CityStrategyBuilderService.RequestSaving();
    }

    private void SelectGroup()
    {
        if (_skCanvasView == null)
        {
            return;
        }

        CityStrategyBuilderService.SelectCityMapEntityGroup();
        _skCanvasView.Invalidate();
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
        CityStrategyBuilderService.RenderScene(canvas);
    }

    private void ToggleLeftPanel(bool toggled)
    {
        _leftPanelIsVisible = toggled;
    }

    private void ToggleRightPanel(bool toggled)
    {
        _rightPanelIsVisible = toggled;
    }

    private void ZoomIn()
    {
        if (_skCanvasView == null)
        {
            return;
        }

        if (CityPlannerInteractionManager.Zoom(_canvasSize.Width / 2, _canvasSize.Height / 2, -100))
        {
            _skCanvasView.Invalidate();
        }
    }

    private void ZoomOut()
    {
        if (_skCanvasView == null)
        {
            return;
        }

        if (CityPlannerInteractionManager.Zoom(_canvasSize.Width / 2, _canvasSize.Height / 2, 100))
        {
            _skCanvasView.Invalidate();
        }
    }

    private void NavigateToDashboard()
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITY_STRATEGIES_DASHBOARD_PATH, false, true);
    }

    private async Task OnCreateTimelineItem(CityStrategyTimelineItemCreateRequest request)
    {
        if (request.Type == CityStrategyNewTimelineItemType.LayoutImport)
        {
            var cities = await CityStrategyBuilderService.GetCities();
            if (cities.Count == 0)
            {
                await DialogService.ShowMessageBox(
                    null,
                    Loc[FogResource.CityStrategy_NoCityForImport],
                    Loc[FogResource.Common_Ok]);
            }
            else
            {
                var parameters = new DialogParameters<CityPickerDialog>
                {
                    {d => d.Cities, cities},
                };
                var dialog = await DialogService.ShowAsync<CityPickerDialog>(null, parameters,
                    GetDefaultDialogOptions());
                var result = await dialog.Result;
                if (result is not {Canceled: true})
                {
                    request.ExistingCityId = result?.Data as string;
                    await CityStrategyBuilderService.CreateTimelineItemAsync(request);
                }
            }
        }
        else
        {
            await CityStrategyBuilderService.CreateTimelineItemAsync(request);
        }
    }

    private async Task OpenItemTitleDialog(CityStrategyTimelineItemBase item)
    {
        var parameters = new DialogParameters<TimelineItemTitleDialog>
        {
            {d => d.Data, item},
        };
        var dialog = await DialogService.ShowAsync<TimelineItemTitleDialog>(null, parameters,
            GetDefaultDialogOptions());
        await dialog.Result;
        await Save();
    }

    private Task OnDeleteTimelineItem(CityStrategyTimelineItemBase item)
    {
        return CityStrategyBuilderService.DeleteTimelineItem(item);
    }

    private Task OnEditTimelineItem(CityStrategyTimelineItemBase item)
    {
        return OpenItemTitleDialog(item);
    }

    private Task OnSelectTimelineItem(string itemId)
    {
        return CityStrategyBuilderService.SelectTimelineItem(itemId);
    }

    private static DialogOptions GetDefaultDialogOptions()
    {
        return new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            BackgroundClass = "dialog-blur-bg",
            NoHeader = true,
            CloseOnEscapeKey = true,
            Position = DialogPosition.TopCenter,
        };
    }
}

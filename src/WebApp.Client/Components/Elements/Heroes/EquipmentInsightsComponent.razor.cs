using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Refit;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.Heroes;

public partial class EquipmentInsightsComponent : ComponentBase, IAsyncDisposable
{
    private IReadOnlyCollection<EquipmentInsightsViewModel>? _allEquipmentInsights;
    private CancellationTokenSource? _cts;
    private IReadOnlyCollection<EquipmentInsightsViewModel> _equipmentInsights = [];
    private IReadOnlyCollection<EquipmentSlotTypeViewModel> _equipmentSlotTypes = [];
    private bool _isLoading = true;
    private EquipmentSlotType _selectedSlotType;

    [Inject]
    private IEquipmentUiService EquipmentUiService { get; set; }

    [Inject]
    private IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    public ILogger<UnitBattlesComponent> Logger { get; set; }

    [Parameter]
    [EditorRequired]
    public required string UnitId { get; set; }

    public async ValueTask DisposeAsync()
    {
        if (_cts != null)
        {
            await _cts.CancelAsync();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        _equipmentSlotTypes = await EquipmentUiService.GetEquipmentSlotTypesAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        _selectedSlotType = _equipmentSlotTypes.First().SlotType;
        StateHasChanged();
    }

    private async Task OnExpanded(bool expanded)
    {
        if (expanded)
        {
            await GetEquipment();
            OnEquipmentSlotTypeChanged(_selectedSlotType);
        }
    }

    private async Task GetEquipment()
    {
        if (_allEquipmentInsights != null)
        {
            return;
        }

        if (_cts != null)
        {
            await _cts.CancelAsync();
        }

        _cts = new CancellationTokenSource();
        _isLoading = true;
        StateHasChanged();
        try
        {
            _allEquipmentInsights = await EquipmentUiService.GetEquipmentInsights(UnitId, _cts.Token);
        }
        catch (OperationCanceledException _)
        {
            return;
        }
        catch (ApiException apiEx) when (apiEx.InnerException is TaskCanceledException)
        {
            return;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unexpected error.");
        }

        _isLoading = false;
        StateHasChanged();
    }

    private void OnEquipmentSlotTypeChanged(EquipmentSlotType slotType)
    {
        var all = _allEquipmentInsights ?? Enumerable.Empty<EquipmentInsightsViewModel>();
        _equipmentInsights = all.Where(x => x.EquipmentSlotType == slotType).ToList();
        _selectedSlotType = slotType;
    }
}

using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Tools;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner;

public partial class CityPlannerBuildingCostCalculator : ComponentBase
{
    private BuildingMultilevelCostViewModel? _costs;

    private CancellationTokenSource _cts = new ();
    private BuildingLevelSpecs _fromLevel;
    private IList<BuildingLevelSpecs>? _fromLevels;
    private BuildingGroupViewModel? _selectedBuildingGroupDetails;
    private BuildingLevelSpecs? _toLevel;
    private IList<BuildingLevelSpecs>? _toLevels;

    [Parameter]
    public required BuildingGroup BuildingGroup { get; set; }

    [Parameter]
    public required string BuildingName { get; set; }

    [Parameter]
    public required CityId CityId { get; set; }

    [Inject]
    private ICityUiService CityUiService { get; set; }

    [Parameter]
    public int FromLevel { get; set; }

    [Inject]
    private IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    private IToolsUiService ToolsUiService { get; set; }

    private void ToLevelOnValueChanged(BuildingLevelSpecs selectedLevel)
    {
        _toLevel = selectedLevel;

        CalculateCost();
    }

    private void CalculateCost()
    {
        if (_toLevels == null || _toLevel == null)
        {
            _costs = null;
            return;
        }

        _costs = ToolsUiService.CalculateBuildingMultiLevelCost(_selectedBuildingGroupDetails!, _fromLevel!.Level,
            _toLevel.Level);
    }

    private List<BuildingLevelSpecs>? GetToLevels()
    {
        var toLevels = _selectedBuildingGroupDetails!.Buildings
            .Where(b => b.Level > _fromLevel!.Level &&
                        (b.ConstructionComponent != null || b.UpgradeComponent != null))
            .Select(b => b.LevelSpecs)
            .OrderBy(x => x.Level)
            .ToList();
        return toLevels.Count == 0 ? null : toLevels;
    }

    private void Reset()
    {
        _fromLevels = null;
        _toLevels = null;
        _costs = null;
    }

    private void FromLevelOnValueChanged(BuildingLevelSpecs selectedLevel)
    {
        _fromLevel = selectedLevel;

        _toLevels = GetToLevels();
        if (_toLevel != null)
        {
            if (_fromLevel.Level >= _toLevel.Level)
            {
                _toLevel = _toLevels?.First();
            }
        }
        else
        {
            _toLevel = _toLevels?.First();
        }

        CalculateCost();
    }

    protected override async Task OnParametersSetAsync()
    {
        await _cts.CancelAsync();
        _cts = new CancellationTokenSource();

        Reset();

        if (BuildingGroup == BuildingGroup.Undefined)
        {
            return;
        }

        try
        {
            _selectedBuildingGroupDetails =
                await CityUiService.GetBuildingGroupAsync(CityId, BuildingGroup, _cts.Token);
        }
        catch
        {
            return;
        }

        if (_selectedBuildingGroupDetails == null)
        {
            return;
        }

        var buildings = _selectedBuildingGroupDetails.Buildings
            .Where(b => b.ConstructionComponent != null || b.UpgradeComponent != null)
            .Select(b => b.LevelSpecs)
            .OrderBy(x => x.Level)
            .ToList();

        _fromLevels = new List<BuildingLevelSpecs>() {BuildingLevelSpecs.ZeroLevel}.Concat(buildings)
            .ToList();
        if (_fromLevels.Count > 1)
        {
            _fromLevel = _fromLevels.First(src => src.Level == FromLevel);
            _toLevels = GetToLevels();
            _toLevel = _toLevels?.First();
            CalculateCost();
        }
        else
        {
            _fromLevels = null;
            _toLevels = null;
            _costs = null;
        }

        // We need this for a proper rendering in case our async calls in this method run synchronously.
        await Task.Yield();
    }
}

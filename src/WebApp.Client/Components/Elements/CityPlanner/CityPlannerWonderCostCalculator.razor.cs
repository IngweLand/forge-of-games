using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner;

public partial class CityPlannerWonderCostCalculator : ComponentBase
{
    private const int MAX_LEVELS = 50;
    
    private IReadOnlyCollection<IconLabelItemViewModel> _costs = [];
    private int _fromLevel;
    private readonly IReadOnlyCollection<int> _fromLevels = Enumerable.Range(0, MAX_LEVELS + 1).ToList();
    private int _toLevel = 1;
    private IList<int> _toLevels = Enumerable.Range(1, MAX_LEVELS).ToList();
    private bool _toLevelsVisible = true;

    [Inject]
    private ICityPlanner CityPlanner { get; set; }

    [Parameter]
    [EditorRequired]
    public required int FromLevel { get; set; }

    [Inject]
    private IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    private IToolsUiService ToolsUiService { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        FromLevelOnValueChanged(FromLevel);
    }

    private void ToLevelOnValueChanged(int selectedLevel)
    {
        _toLevel = selectedLevel;

        CalculateCost();
    }

    private void CalculateCost()
    {
        if (_fromLevel >= MAX_LEVELS)
        {
            _costs = [];
            _toLevelsVisible = false;
            return;
        }

        _toLevelsVisible = true;
        _costs = ToolsUiService.CalculateWonderLevelsCost(CityPlanner.CityMapState.CityWonder!, _fromLevel, _toLevel);
    }

    private List<int> GetToLevels()
    {
        return Enumerable.Range(_fromLevel + 1, MAX_LEVELS - _fromLevel).ToList();
    }

    private void FromLevelOnValueChanged(int selectedLevel)
    {
        _fromLevel = selectedLevel;

        _toLevels = GetToLevels();
        if (_toLevel <= _fromLevel)
        {
            _toLevel = _fromLevel + 1;
            if (_toLevel > MAX_LEVELS)
            {
                _toLevel = MAX_LEVELS;
            }
        }

        CalculateCost();
    }
}

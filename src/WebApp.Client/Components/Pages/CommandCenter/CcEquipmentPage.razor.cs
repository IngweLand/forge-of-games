using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcEquipmentPage : CommandCenterPageBase
{
    private TableData<EquipmentItemViewModel> _defaultTableData;
    private IReadOnlyCollection<EquipmentItemViewModel>? _equipment;

    [Inject]
    private ICcEquipmentUiService EquipmentUiService { get; set; }

    protected override async Task HandleOnInitializedAsync()
    {
        await base.HandleOnInitializedAsync();

        _equipment = await EquipmentUiService.GetEquipmentAsync();

        _defaultTableData = new TableData<EquipmentItemViewModel> {TotalItems = _equipment.Count, Items = _equipment};
    }

    private Task<TableData<EquipmentItemViewModel>> GetItems(TableState state, CancellationToken token)
    {
        if (_equipment == null)
        {
            return Task.FromResult(new TableData<EquipmentItemViewModel>());
        }

        if (state.SortDirection == SortDirection.None)
        {
            return Task.FromResult(_defaultTableData);
        }

        var data = state.SortLabel switch
        {
            nameof(EquipmentItemViewModel.EquipmentSlotType) => _equipment.OrderByDirection(state.SortDirection,
                x => x.EquipmentSlotType),
            nameof(EquipmentItemViewModel.EquipmentSet) => _equipment.OrderByDirection(state.SortDirection,
                x => x.EquipmentSet),
            nameof(EquipmentItemViewModel.StarCount) => _equipment.OrderByDirection(state.SortDirection,
                x => x.StarCount),
            nameof(EquipmentItemViewModel.Level) => _equipment.OrderByDirection(state.SortDirection, x => x.Level),
            nameof(EquipmentItemViewModel.EquippedOnHero) => _equipment.OrderByDirection(state.SortDirection,
                x => x.EquippedOnHero),
            nameof(EquipmentItemViewModel.MainAttack) => HandleAttributeSort(_equipment, x => x.MainAttack,
                state.SortDirection),
            nameof(EquipmentItemViewModel.MainDefense) => HandleAttributeSort(_equipment, x => x.MainDefense,
                state.SortDirection),
            nameof(EquipmentItemViewModel.SubAttack) => HandleSubAttributeSort(_equipment, x => x.SubAttack,
                state.SortDirection),
            nameof(EquipmentItemViewModel.SubAttackAmp) => HandleSubAttributeSort(_equipment, x => x.SubAttackAmp,
                state.SortDirection),
            nameof(EquipmentItemViewModel.SubDefense) => HandleSubAttributeSort(_equipment, x => x.SubDefense,
                state.SortDirection),
            nameof(EquipmentItemViewModel.SubDefenseAmp) => HandleSubAttributeSort(_equipment, x => x.SubDefenseAmp,
                state.SortDirection),
            nameof(EquipmentItemViewModel.SubHitPoints) => HandleSubAttributeSort(_equipment, x => x.SubHitPoints,
                state.SortDirection),
            nameof(EquipmentItemViewModel.SubHitPointsAmp) => HandleSubAttributeSort(_equipment, x => x.SubHitPointsAmp,
                state.SortDirection),
            nameof(EquipmentItemViewModel.SubBaseDamageAmp) => HandleSubAttributeSort(_equipment,
                x => x.SubBaseDamageAmp,
                state.SortDirection),
            nameof(EquipmentItemViewModel.SubCritDamage) => HandleSubAttributeSort(_equipment, x => x.SubCritDamage,
                state.SortDirection),
            nameof(EquipmentItemViewModel.SubInitialFocusInSecondsBonus) => HandleSubAttributeSort(_equipment,
                x => x.SubInitialFocusInSecondsBonus,
                state.SortDirection),
            nameof(EquipmentItemViewModel.SubCritChance) => HandleSubAttributeSort(_equipment, x => x.SubCritChance,
                state.SortDirection),
            nameof(EquipmentItemViewModel.SubAttackSpeed) => HandleSubAttributeSort(_equipment, x => x.SubAttackSpeed,
                state.SortDirection),
            _ => _equipment,
        };

        return Task.FromResult(new TableData<EquipmentItemViewModel> {TotalItems = _equipment.Count, Items = data});
    }

    private static (IEnumerable<T> Matched, IEnumerable<T> NotMatched) SplitByCondition<T>(IEnumerable<T> source,
        Func<T, bool> predicate)
    {
        var groups = source.ToLookup(predicate);
        return (groups[true], groups[false]);
    }

    private static IEnumerable<EquipmentItemViewModel> HandleAttributeSort(
        IEnumerable<EquipmentItemViewModel> source,
        Func<EquipmentItemViewModel, EquipmentItemAttributeViewModel?> propertySelector,
        SortDirection sortDirection)

    {
        var allGrouped = SplitByCondition(source, x => propertySelector(x) != null);
        var sorted = allGrouped.Matched.OrderByDirection(sortDirection, x => propertySelector(x)!.Value);

        return sorted.Concat(allGrouped.NotMatched);
    }

    private static IEnumerable<EquipmentItemViewModel> HandleSubAttributeSort(
        IEnumerable<EquipmentItemViewModel> source,
        Func<EquipmentItemViewModel, EquipmentItemSubAttributeViewModel?> propertySelector,
        SortDirection sortDirection)

    {
        var allGrouped = SplitByCondition(source, x => propertySelector(x) != null);
        var mainGrouped = SplitByCondition(allGrouped.Matched, x => propertySelector(x)!.Value != null);
        var sorted = mainGrouped.Matched.OrderByDirection(sortDirection, x => propertySelector(x)!.Value);

        return sorted
            .Concat(mainGrouped.NotMatched.OrderBy(x => propertySelector(x)!.UnlockedAtLevel))
            .Concat(allGrouped.NotMatched);
    }
}

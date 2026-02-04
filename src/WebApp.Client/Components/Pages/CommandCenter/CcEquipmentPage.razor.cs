using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcEquipmentPage : CommandCenterPageBase
{
    private IReadOnlyCollection<(StatAttribute StatAttribute, string Title)> _attributes = [];
    private IReadOnlyCollection<EquipmentItemViewModel>? _equipment;

    [Inject]
    private IEquipmentUiService EquipmentUiService { get; set; }

    protected override async Task HandleOnInitializedAsync()
    {
        await base.HandleOnInitializedAsync();

        _equipment = await EquipmentUiService.GetEquipmentAsync();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _attributes = new List<(StatAttribute, string )>
        {
            (StatAttribute.Attack, Loc[FogResource.UnitStats_AttackAbbrev]),
            (StatAttribute.AttackBonus, Loc[FogResource.UnitStats_AttackPercentAbbrev]),
            (StatAttribute.Defense, Loc[FogResource.UnitStats_DefenseAbbrev]),
            (StatAttribute.DefenseBonus, Loc[FogResource.UnitStats_DefensePercentAbbrev]),
            (StatAttribute.MaxHitPoints, Loc[FogResource.UnitStats_MaxHitPointsAbbrev]),
            (StatAttribute.MaxHitPointsBonus, Loc[FogResource.UnitStats_MaxHitPointsPercentAbbrev]),
            (StatAttribute.BaseDamageBonus, Loc[FogResource.UnitStats_BaseDamagePercentAbbrev]),
            (StatAttribute.CritChance, Loc[FogResource.UnitStats_CritChanceAbbrev]),
            (StatAttribute.CritDamage, Loc[FogResource.UnitStats_CritDamageAbbrev]),
            (StatAttribute.InitialFocusInSecondsBonus, Loc[FogResource.UnitStats_InitialFocusInSecondsBonusAbbrev]),
            (StatAttribute.AttackSpeed, Loc[FogResource.UnitStats_AttackSpeedAbbrev]),
        };
    }

    private static EquipmentItemAttributeViewModel? GetMainAttributeProperty(
        EquipmentItemViewModel item, StatAttribute statAttribute)
    {
        if (item.MainAttribute.StatAttribute == statAttribute)
        {
            return item.MainAttribute;
        }

        return null;
    }

    private static EquipmentItemSubAttributeViewModel? GetSubAttributeProperty(
        EquipmentItemViewModel item, StatAttribute statAttribute)
    {
        return item.SubAttributes.GetValueOrDefault(statAttribute, null);
    }

    private static MarkupString GetMainAttributeValue(EquipmentItemViewModel item, StatAttribute statAttribute)
    {
        if (item.MainAttribute.StatAttribute == statAttribute)
        {
            return (MarkupString) (item.MainAttribute.FormattedValue ?? string.Empty);
        }

        return new MarkupString();
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

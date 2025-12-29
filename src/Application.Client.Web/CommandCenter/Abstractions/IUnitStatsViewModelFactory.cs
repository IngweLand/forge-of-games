using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface IUnitStatsViewModelFactory
{
    IReadOnlyCollection<IconLabelItemViewModel> CreateMainStatsItems(IReadOnlyDictionary<UnitStatType, float> stats);

    IReadOnlyCollection<UnitStatBreakdownViewModel> CreateStatsBreakdownItems(
        IReadOnlyDictionary<UnitStatType, IReadOnlyDictionary<UnitStatSource, float>> stats);

    IReadOnlyCollection<IconLabelsItemViewModel> CreateStatsItems(IReadOnlyDictionary<UnitStatType, float> stats,
        IReadOnlyDictionary<UnitStatType, string> unitStatNames);
}

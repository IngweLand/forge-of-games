namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class EncounterViewModel
{
    public IList<EncounterRewardViewModel> FirstTimeComletionBonus { get; init; }
    public IList<EncounterRewardViewModel> Rewards { get; init; }
    public required string Title { get; init; }
    public required IReadOnlyCollection<BattleWaveViewModel> Waves { get; init; }
    public required IconLabelItemViewModel AvailableHeroSlots { get; init; }
}

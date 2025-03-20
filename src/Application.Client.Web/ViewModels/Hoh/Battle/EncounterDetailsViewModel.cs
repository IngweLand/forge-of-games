namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class EncounterDetailsViewModel
{
    public required IconLabelItemViewModel AvailableHeroSlots { get; init; }
    public IList<EncounterRewardViewModel> FirstTimeComletionBonus { get; init; }

    public IReadOnlyCollection<string> RequiredHeroClassIconUrls { get; init; } = Array.Empty<string>();

    public IReadOnlyCollection<string> RequiredHeroTypeIconUrls { get; init; } = Array.Empty<string>();

    public IList<EncounterRewardViewModel> Rewards { get; init; }
    public required IReadOnlyCollection<BattleWaveViewModel> Waves { get; init; }
}
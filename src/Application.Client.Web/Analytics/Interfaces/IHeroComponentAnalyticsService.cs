using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;

public interface IHeroComponentAnalyticsService
{
    void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object>? eventParams = null);

    void TrackHeroLevelChange(IReadOnlyDictionary<string, object> baseParams,
        HeroProfileIdentifier profileIdentifier);

    void TrackProgressionCalculatorEvent(IReadOnlyDictionary<string, object> baseParams,
        HeroLevelSpecs targetLevel);

    void TrackBattleNavigation(IReadOnlyDictionary<string, object> baseParams, UnitBattleViewModel battle);
}

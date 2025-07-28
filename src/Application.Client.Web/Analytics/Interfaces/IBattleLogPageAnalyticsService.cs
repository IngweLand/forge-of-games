using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;

public interface IBattleLogPageAnalyticsService
{
    void TrackEvent(string eventName, IReadOnlyDictionary<string, object> baseParams,
        Dictionary<string, object>? eventParams = null);

    void TrackFormChange(BattleSearchRequest request, IReadOnlyDictionary<string, object> baseParams);

    void TrackSquadProfileView(string eventName, IReadOnlyDictionary<string, object> baseParams, string unitId);
}

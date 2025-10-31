namespace Ingweland.Fog.Application.Core.Constants;

public static class FogConstants
{
    public const int NAME_MAX_CHARACTERS = 40;
    public const int DEFAULT_STATS_PAGE_SIZE = 10;
    public const int CITY_PLANNER_VERSION = 4;
    public const int CC_UI_PROFILE_SCHEME_VERSION = 1;
    public const int COMMAND_CENTER_VERSION = 1;
    public const int MAX_TOP_HERO_LEVELS_TO_RETURN = 5;
    public const int MAX_MOST_POPULAR_HEROES_TO_RETURN = 10;
    public const int TOP_HEROES_LOOKBACK_DAYS = 14;
    public const int ALLIANCE_STALE_THRESHOLD_DAYS = 1;
    public const int ALLIANCE_NAME_MIN_IN_GAME_SEARCH_STRING_LENGTH = 3;
    public const int SHARED_SUBMISSION_ID_VALIDITY_DAYS = 1;
    public const int MAX_DISPLAYED_ATH_EVENTS = 10;

    public static readonly int MaxHohCitySnapshots = 5;
    public static readonly int DisplayedStatsDays = 30;
    public static readonly int MaxDisplayedBattles = 30;
    public static readonly int MaxDisplayedUnitBattles = 30;
    public static readonly int MaxPlayerCitySnapshotSearchResults = 20;
    public static readonly TimeSpan DefaultHohDataEntityCacheTime = TimeSpan.FromHours(24);
}

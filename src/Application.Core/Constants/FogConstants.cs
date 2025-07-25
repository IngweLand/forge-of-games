namespace Ingweland.Fog.Application.Core.Constants;

public static class FogConstants
{
    public const int NAME_MAX_CHARACTERS = 40;
    public const int DEFAULT_STATS_PAGE_SIZE = 10;
    public const int CITY_PLANNER_VERSION = 3;
    public const int CC_UI_PROFILE_SCHEME_VERSION = 1;
    public const int COMMAND_CENTER_VERSION = 1;
    public const int MAX_TOP_HERO_LEVELS_TO_RETURN = 5;
    public const int MAX_MOST_POPULAR_HEROES_TO_RETURN = 10;
    public const int TOP_HEROES_LOOKBACK_DAYS = 30;

    public const int PLAYER_PROFILE_TOP_HEROES_COUNT = 5;
    public static readonly int MaxHohCitySnapshots = 5;
    public static readonly int DisplayedStatsDays = 30;
    public static readonly int DefaultPlayerProfileDisplayedBattleCount = 2;
    public static readonly int MaxDisplayedBattles = 30;
    public static readonly int MaxDisplayedUnitBattles = 30;
    public static readonly int MaxPlayerCitySnapshotSearchResults = 20;
    public static readonly TimeSpan DefaultHohDataEntityCacheTime = TimeSpan.FromHours(24);
}

namespace Ingweland.Fog.Application.Client.Web.Analytics;

public static class AnalyticsParams
{
    public const string BUTTON_NAME = "btn_name";
    public const string LOCATION = "location";
    public const string SOURCE = "source";
    public const string FORM_NAME = "form_name";
    public const string BUTTON_ID = "btn_id";
    public const string FOG_PLAYER_ID = "fog_player_id";
    public const string FOG_BATTLE_ID = "fog_battle_id";
    public const string FOG_ALLIANCE_ID = "fog_alliance_id";
    public const string UNIT_ID = "unit_id";
    public const string STATE = "state";

    public static class Values
    {
        public static class Locations
        {
            public const string PLAYER_PROFILE = "player_profile";
            public const string BATTLE_LOG = "battle_log";
            public const string CITY_PLANNER_APP = "city_planner_app";
            public const string CITY_INSPIRATIONS = "city_inspirations";
        }

        public static class Sources
        {
            public const string TOP_HEROES = "top_heroes";
            public const string PVP_BATTLE = "pvp_battle";
            public const string PLAYER_RANKING_CHART = "player_ranking_chart";
            public const string PLAYER_PVP_RANKING_CHART = "player_pvp_ranking_chart";
            public const string PLAYER_INFO_COMPONENT = "player_info_component";
            public const string ALLIANCE_LIST = "alliance_list";
        }
        
        public static class States
        {
            public const string ON = "on";
            public const string OFF = "off";
        }
    }
}

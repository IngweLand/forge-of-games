namespace Ingweland.Fog.Application.Core.Helpers;

public static class FogUrlBuilder
{
    private static string BuildPath(params string[] segments)
    {
        var cleanSegments = segments
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim('/'));

        return "/" + string.Join("/", cleanSegments);
    }

    public static class ApiRoutes
    {
        private const string BASE_STATS_PATH = "stats";
        public const string PLAYERS_TEMPLATE = "/" + BASE_STATS_PATH + "/worlds/{worldId}/players";

        public const string PLAYER_TEMPLATE_REFIT =
            "/" + BASE_STATS_PATH + "/worlds/{worldId}/players/{inGamePlayerId}";

        public const string PLAYER_TEMPLATE = "/" + BASE_STATS_PATH + "/worlds/{worldId}/players/{inGamePlayerId:int}";
    }

    public static class PageRoutes
    {
        public const string BASE_ABOUT_PATH = "/about";
        public const string BASE_BUILDINGS_PATH = "/buildings";
        public const string BASE_CAMPAIGN_PATH = "/campaign";
        public const string BASE_CITY_PLANNER_PATH = "/city-planner";
        public const string BASE_COMMAND_CENTER_PATH = "/command-center";
        public const string BASE_HEROES_PATH = "/heroes";
        public const string BASE_STATS_HUB_PATH = "/stats-hub";
        public const string BASE_TOOLS_PATH = "/tools";
        public const string BASE_TREASURE_HUNT_CAMPAIGN_PATH = "/treasure-hunt-campaign";
        public const string BASE_WONDERS_PATH = "/wonders";
        public const string HELP_COMMAND_CENTER_PATH = "/help/command-center";
        public const string PLAYER_TEMPLATE = BASE_STATS_HUB_PATH + "/worlds/{worldId}/players/{playerId:int}";
        public const string SUPPORT_US_PATH = "/support-us";
        public const string WORLD_TEMPLATE = BASE_STATS_HUB_PATH + "/worlds/{worldId}";

        public static string Player(string worldId, int playerId)
        {
            if (string.IsNullOrWhiteSpace(worldId))
            {
                throw new ArgumentNullException(nameof(worldId));
            }

            return BuildPath(BASE_STATS_HUB_PATH, "worlds", worldId, "players", playerId.ToString());
        }
        
        public static string World(string worldId)
        {
            if (string.IsNullOrWhiteSpace(worldId))
            {
                throw new ArgumentNullException(nameof(worldId));
            }

            return BuildPath(BASE_STATS_HUB_PATH, "worlds", worldId);
        }
    }
}

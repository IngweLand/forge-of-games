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
        private const string BASE_BATTLES_PATH = "battles";

        public const string CAMPAIGN_CONTINENTS_BASIC_DATA_PATH = "/campaign/continents/basicData";
        public const string CAMPAIGN_REGION_TEMPLATE = "/campaign/regions/{regionId}";
        public const string CAMPAIGN_REGION_BASIC_DATA_TEMPLATE = "/campaign/regions/{regionId}/basicData";

        public const string TREASURE_HUNT_STAGE_TEMPLATE_REFIT = "/ath/difficulties/{difficulty}/stages/{stageIndex}";
        public const string TREASURE_HUNT_DIFFICULTIES_PATH = "/ath/difficulties";
        public const string TREASURE_HUNT_STAGE_TEMPLATE = "/ath/difficulties/{difficulty:int}/stages/{stageIndex:int}";
        public const string TREASURE_HUNT_ENCOUNTERS_BASIC_DATA_PATH = "/ath/encounter/basicData";

        public const string WIKI_EXTRACT = "/wiki/extract";

        public const string PLAYERS_TEMPLATE = "/" + BASE_STATS_PATH + "/worlds/{worldId}/players";
        public const string PLAYER_TEMPLATE = "/" + BASE_STATS_PATH + "/players/{playerId:int}";
        public const string PLAYER_TEMPLATE_REFIT = "/" + BASE_STATS_PATH + "/players/{playerId}";

        public const string ALLIANCES_TEMPLATE = "/" + BASE_STATS_PATH + "/worlds/{worldId}/alliances";
        public const string ALLIANCE_TEMPLATE = "/" + BASE_STATS_PATH + "/alliances/{allianceId:int}";
        public const string ALLIANCE_TEMPLATE_REFIT = "/" + BASE_STATS_PATH + "/alliances/{allianceId}";

        public const string BATTLE_LOG_SEARCH = "/battle-log/search";
        public const string BATTLE_STATS_TEMPLATE = "/" + BASE_BATTLES_PATH + "/stats/{battleStatsId:int}";
        public const string BATTLE_STATS_TEMPLATE_REFIT = "/" + BASE_BATTLES_PATH + "/stats/{battleStatsId}";
        public const string UNIT_BATTLES_TEMPLATE = "/units/{unitId}/battles";
    }

    public static class PageRoutes
    {
        public const string BASE_ABOUT_PATH = "/about";
        public const string BASE_BUILDINGS_PATH = "/buildings";
        public const string BASE_CAMPAIGN_PATH = "/campaign";
        public const string BASE_CITY_PLANNER_PATH = "/city-planner";
        public const string BASE_COMMAND_CENTER_PATH = "/command-center";
        public const string COMMAND_CENTER_PROFILES_PATH = BASE_COMMAND_CENTER_PATH + "/profiles";
        public const string COMMAND_CENTER_HERO_PLAYGROUNDS_PATH = BASE_COMMAND_CENTER_PATH + "/playgrounds/heroes";
        public const string COMMAND_CENTER_EQUIPMENT_PATH = BASE_COMMAND_CENTER_PATH + "/equipment";
        public const string BASE_HEROES_PATH = "/heroes";
        public const string BASE_STATS_HUB_PATH = "/stats-hub";
        public const string BASE_TOOLS_PATH = "/tools";
        public const string BASE_TREASURE_HUNT_PATH = "/treasure-hunt";
        public const string BASE_WONDERS_PATH = "/wonders";
        public const string BASE_HELP_PATH = "/help";
        public const string HELP_COMMAND_CENTER_PATH = BASE_HELP_PATH + "/command-center";
        public const string HELP_HERO_PLAYGROUNDS_PATH = HELP_COMMAND_CENTER_PATH + "/hero-playgrounds";
        public const string HELP_EQUIPMENT_PATH = BASE_HELP_PATH + "/equipment";
        public const string HELP_CITY_PLANNER_SNAPSHOTS_PATH = BASE_HELP_PATH + "/city-planner-snapshots";
        public const string HELP_BROWSER_EXTENSION_PATH = BASE_HELP_PATH + "/browser-extension";
        public const string HELP_IMPORTING_IN_GAME_DATA_PATH = BASE_HELP_PATH + "/importing-hoh-data";
        public const string HELP_STATS_HUB_PATH = BASE_HELP_PATH + "/stats-hub";
        public const string HELP_BATTLE_LOG_PATH = BASE_HELP_PATH + "/battle-log";
        public const string HERO_TEMPLATE = BASE_HEROES_PATH + "/{heroId}";
        public const string CAMPAIGN_REGION_TEMPLATE = BASE_CAMPAIGN_PATH + "/region/{regionId}";
        public const string BUILDING_TEMPLATE = BASE_BUILDINGS_PATH + "/{cityId}/{buildingGroup}";
        public const string RESEARCH_CALCULATOR_PATH = BASE_TOOLS_PATH + "/research-calculator";
        public const string WONDER_COST_CALCULATOR_PATH = BASE_TOOLS_PATH + "/wonder-cost-calculator";
        public const string BUILDING_COST_CALCULATOR_PATH = BASE_TOOLS_PATH + "/building-cost-calculator";
        public const string WONDER_TEMPLATE = BASE_WONDERS_PATH + "/{wonderId}";

        public const string TREASURE_HUNT_STAGE_TEMPLATE =
            BASE_TREASURE_HUNT_PATH + "/{difficulty:int}/{stageIndex:int}";

        public const string SUPPORT_US_PATH = "/support-us";
        public const string WORLD_PLAYERS_TEMPLATE = BASE_STATS_HUB_PATH + "/worlds/{worldId}/players";
        public const string WORLD_ALLIANCES_TEMPLATE = BASE_STATS_HUB_PATH + "/worlds/{worldId}/alliances";
        public const string PLAYER_TEMPLATE = BASE_STATS_HUB_PATH + "/players/{playerId:int}";
        public const string ALLIANCE_TEMPLATE = BASE_STATS_HUB_PATH + "/alliances/{allianceId:int}";
        public const string FOG_GITHUB_URL = "https://github.com/IngweLand/forge-of-games";
        public const string HOH_HELPER_GITHUB_URL = "https://github.com/IngweLand/hoh-helper";

        public const string HOH_HELPER_CHROME_WEBSTORE_URL =
            "https://chromewebstore.google.com/detail/hoh-helper-forge-of-games/almhmnmbpfaonomgaconnmcogadnjndf";

        public const string HOH_HELPER_RELEASES_GITHUB_URL = HOH_HELPER_GITHUB_URL + "/releases";
        public const string FOG_DISCORD_URL = "https://discord.gg/4vFeeh7CZn";
        public const string CITIES_STATS_PATH = "/cities-stats";
        public const string BATTLE_LOG_PATH = "/battle-log";

        public static string Player(int id)
        {
            return PLAYER_TEMPLATE.Replace("{playerId:int}", id.ToString());
        }

        public static string HeroPlayground(string heroId)
        {
            return $"{COMMAND_CENTER_HERO_PLAYGROUNDS_PATH}/{heroId}";
        }

        public static string Alliance(int id)
        {
            return ALLIANCE_TEMPLATE.Replace("{allianceId:int}", id.ToString());
        }

        public static string SearchAlliance(string worldId, string name)
        {
            return $"{WORLD_ALLIANCES_TEMPLATE.Replace("{worldId}", worldId)}?name={Uri.EscapeDataString(name)}";
        }

        public static string TreasureHuntStage(int difficulty, int stageIndex)
        {
            return BuildPath(BASE_TREASURE_HUNT_PATH, difficulty.ToString(), stageIndex.ToString());
        }

        public static string WorldPlayers(string worldId)
        {
            if (string.IsNullOrWhiteSpace(worldId))
            {
                throw new ArgumentNullException(nameof(worldId));
            }

            return WORLD_PLAYERS_TEMPLATE.Replace("{worldId}", worldId);
        }

        public static string WorldAlliances(string worldId)
        {
            if (string.IsNullOrWhiteSpace(worldId))
            {
                throw new ArgumentNullException(nameof(worldId));
            }

            return WORLD_ALLIANCES_TEMPLATE.Replace("{worldId}", worldId);
        }
    }
}

using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.WebApp.Client.Models;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.WebApp.Client.Services;

public class MainMenuService(NavigationManager navigationManager, IStringLocalizer<FogResource> localizer)
    : IMainMenuService
{
    public IReadOnlyCollection<NavMenuItem> GetMainMenuItems()
    {
        return new List<NavMenuItem>
        {
            new()
            {
                Href = "/",
                ResourceKey = FogResource.Navigation_Home,
                Icon = GetIconString("images/icon_flat_home.png"),
                Match = NavLinkMatch.All
            },
            new()
            {
                Href = FogUrlBuilder.PageRoutes.BASE_CITY_PLANNER_PATH,
                ResourceKey = FogResource.CityPlanner_Title,
                Icon = GetIconString("images/architecture_30dp_FBE0C6_FILL0_wght700_GRAD200_opsz24.svg")
            },
            new()
            {
                ResourceKey = FogResource.CommandCenter_Title,
                Icon = GetIconString("images/target_32dp_FBE0C6_FILL0_wght400_GRAD0_opsz40.svg"),
                Children = new List<NavMenuItem>
                {
                    new()
                    {
                        Href = FogUrlBuilder.PageRoutes.COMMAND_CENTER_PROFILES_PATH,
                        ResourceKey = FogResource.CommandCenter_Menu_Profiles,
                        Icon = GetIconString("images/target_32dp_FBE0C6_FILL0_wght400_GRAD0_opsz40.svg")
                    },
                    new()
                    {
                        Href = FogUrlBuilder.PageRoutes.COMMAND_CENTER_HERO_PLAYGROUNDS_PATH,
                        ResourceKey = FogResource.CommandCenter_Menu_HeroPlaygrounds,
                        Icon = GetIconString("images/icon_hud_heroes.png")
                    },
                    new()
                    {
                        Href = FogUrlBuilder.PageRoutes.COMMAND_CENTER_EQUIPMENT_PATH,
                        ResourceKey = FogResource.CommandCenter_Menu_Equipment,
                        Icon = GetIconString("images/icon_flat_equipment.png")
                    }
                }
            },
            new()
            {
                Href = FogUrlBuilder.PageRoutes.BASE_STATS_HUB_PATH,
                ResourceKey = FogResource.StatsHub_Title,
                Icon = GetIconString("images/monitoring_24dp_FBE0C6_FILL0_wght400_GRAD0_opsz24.svg")
            },
            new()
            {
                Href = FogUrlBuilder.PageRoutes.BASE_TOOLS_PATH,
                ResourceKey = FogResource.Navigation_Tools,
                Icon = GetIconString("images/construction_28dp_FBE0C6_FILL0_wght700_GRAD200_opsz24.png")
            },
            new()
            {
                Href = FogUrlBuilder.PageRoutes.BASE_HEROES_PATH,
                ResourceKey = FogResource.Hoh_Heroes,
                Icon = GetIconString("images/icon_hud_heroes.png")
            },
            new()
            {
                Href = FogUrlBuilder.PageRoutes.BASE_CAMPAIGN_PATH,
                ResourceKey = FogResource.Navigation_Campaign,
                Icon = GetIconString("images/icon_hud_map.png")
            },
            new()
            {
                Href = FogUrlBuilder.PageRoutes.BASE_TREASURE_HUNT_PATH,
                ResourceKey = FogResource.Navigation_TreasureHunt,
                Icon = GetIconString("images/icon_hud_battle.png")
            },
            new()
            {
                Href = FogUrlBuilder.PageRoutes.BASE_BUILDINGS_PATH,
                ResourceKey = FogResource.Navigation_Buildings,
                Icon = GetIconString("images/icon_hud_build.png")
            },
            new()
            {
                Href = FogUrlBuilder.PageRoutes.BASE_WONDERS_PATH,
                ResourceKey = FogResource.Navigation_Wonders,
                Icon = GetIconString("images/icon_flat_allied_culture.png")
            },
            new()
            {
                Href = FogUrlBuilder.PageRoutes.SUPPORT_US_PATH,
                ResourceKey = FogResource.Navigation_SupportUs,
                Icon = GetIconString("images/volunteer_activism_36dp_FBE0C6_FILL0_wght400_GRAD0_opsz40.svg")
            },
            new()
            {
                Href = FogUrlBuilder.PageRoutes.BASE_ABOUT_PATH,
                ResourceKey = FogResource.Navigation_About,
                Icon = GetIconString("images/info_24dp_FBE0C6_FILL0_wght400_GRAD0_opsz24.svg")
            },
            new()
            {
                Href = FogUrlBuilder.PageRoutes.BASE_HELP_PATH,
                ResourceKey = FogResource.Navigation_Help,
                Icon = GetIconString("images/help_24dp_FBE0C6_FILL1_wght400_GRAD0_opsz24.svg")
            }
        };
    }

    public IReadOnlyCollection<NavMenuItem>? GetCurrentSectionMenuItems()
    {
        var currentPage = new Uri(navigationManager.Uri).AbsolutePath;
        if (currentPage.StartsWith(FogUrlBuilder.PageRoutes.BASE_COMMAND_CENTER_PATH))
        {
            return new List<NavMenuItem>
            {
                new()
                {
                    Href = "/command-center/profiles",
                    ResourceKey = FogResource.CommandCenter_Menu_Profiles
                },
                new()
                {
                    ResourceKey = FogResource.CommandCenter_Menu_Playgrounds,
                    Children = new List<NavMenuItem>
                    {
                        new()
                        {
                            Href = "/command-center/playgrounds/heroes",
                            ResourceKey = FogResource.CommandCenter_Menu_Heroes
                        }
                    }
                }
            };
        }

        return null;
    }

    private string GetIconString(string icon)
    {
        return $"<image width=\"100%\" height=\"100%\" xlink:href=\"{icon}\" preserveAspectRatio=\"xMidYMid meet\"/>";
    }
}
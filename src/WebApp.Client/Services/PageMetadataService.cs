using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.WebApp.Client.Services;

public class PageMetadataService(NavigationManager navigationManager, IStringLocalizer<FogResource> localizer)
    : IPageMetadataService
{
    public PageMetadata GetForCurrentPage()
    {
        var currentPageUri = new Uri(navigationManager.Uri);
        var currentPageAbsolutePath = currentPageUri.AbsolutePath;
        
        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.COMMAND_CENTER_EQUIPMENT_PATH))
        {
            return new PageMetadata
            {
                PageTitle = localizer[FogResource.CommandCenter_Equipment_PageTitle],
                Description = localizer[FogResource.CommandCenter_Equipment_Meta_Description],
                Keywords = localizer[FogResource.CommandCenter_Equipment_Meta_Keywords],
                Title = localizer[FogResource.CommandCenter_Equipment_Title],
                CurrentHomePath = FogUrlBuilder.PageRoutes.COMMAND_CENTER_EQUIPMENT_PATH,
                HelpPagePath = FogUrlBuilder.PageRoutes.HELP_EQUIPMENT_PATH
            };
        }
        
        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.COMMAND_CENTER_HERO_PLAYGROUNDS_PATH))
        {
            return new PageMetadata
            {
                PageTitle = localizer[FogResource.CommandCenter_PageTitle],
                Description = localizer[FogResource.CommandCenter_Meta_Description],
                Keywords = localizer[FogResource.CommandCenter_Meta_Keywords],
                Title = localizer[FogResource.CommandCenter_Menu_HeroPlaygrounds],
                CurrentHomePath = FogUrlBuilder.PageRoutes.COMMAND_CENTER_HERO_PLAYGROUNDS_PATH,
                HelpPagePath = FogUrlBuilder.PageRoutes.HELP_HERO_PLAYGROUNDS_PATH,
            };
        }

        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.BASE_COMMAND_CENTER_PATH))
        {
            return new PageMetadata
            {
                PageTitle = localizer[FogResource.CommandCenter_PageTitle],
                Description = localizer[FogResource.CommandCenter_Meta_Description],
                Keywords = localizer[FogResource.CommandCenter_Meta_Keywords],
                Title = localizer[FogResource.CommandCenter_Title],
                CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_COMMAND_CENTER_PATH,
                HelpPagePath = FogUrlBuilder.PageRoutes.HELP_COMMAND_CENTER_PATH
            };
        }
        
        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.BATTLE_LOG_PATH))
        {
            return new PageMetadata
            {
                PageTitle = localizer[FogResource.StatsHub_BattleLog_PageTitle],
                Description = localizer[FogResource.StatsHub_BattleLog_Meta_Description],
                Keywords = localizer[FogResource.StatsHub_BattleLog_Meta_Keywords],
                Title = localizer[FogResource.StatsHub_BattleLog_Title],
                CurrentHomePath = FogUrlBuilder.PageRoutes.BATTLE_LOG_PATH,
                HelpPagePath = FogUrlBuilder.PageRoutes.HELP_BATTLE_LOG_PATH,
            };
        }

        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.BASE_STATS_HUB_PATH))
        {
            return new PageMetadata
            {
                PageTitle = localizer[FogResource.StatsHub_PageTitle],
                Description = localizer[FogResource.StatsHub_Meta_Description],
                Keywords = localizer[FogResource.StatsHub_Meta_Keywords],
                Title = localizer[FogResource.StatsHub_Title],
                CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_STATS_HUB_PATH,
                HelpPagePath = FogUrlBuilder.PageRoutes.HELP_STATS_HUB_PATH
            };
        }

        // buildings
        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.BASE_BUILDINGS_PATH))
        {
            if (currentPageAbsolutePath == FogUrlBuilder.PageRoutes.BASE_BUILDINGS_PATH)
            {
                return new PageMetadata
                {
                    PageTitle = localizer[FogResource.Buildings_PageTitle],
                    Description = localizer[FogResource.Buildings_Meta_Description],
                    Keywords = localizer[FogResource.Buildings_Meta_Keywords],
                    Title = localizer[FogResource.Navigation_Buildings],
                    CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_BUILDINGS_PATH
                };
            }

            return new PageMetadata
            {
                PageTitle = localizer[FogResource.Building_PageTitle],
                Description = localizer[FogResource.Building_Meta_Description],
                Keywords = localizer[FogResource.Building_Meta_Keywords],
                Title = localizer[FogResource.Navigation_Buildings],
                CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_BUILDINGS_PATH
            };
        }

        // campaign
        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.BASE_CAMPAIGN_PATH))
        {
            if (currentPageAbsolutePath == FogUrlBuilder.PageRoutes.BASE_CAMPAIGN_PATH)
            {
                return new PageMetadata
                {
                    PageTitle = localizer[FogResource.Campaign_PageTitle],
                    Description = localizer[FogResource.Campaign_Meta_Description],
                    Keywords = localizer[FogResource.Campaign_Meta_Keywords],
                    Title = localizer[FogResource.Navigation_Campaign],
                    CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_CAMPAIGN_PATH
                };
            }

            return new PageMetadata
            {
                PageTitle = localizer[FogResource.CampaignRegion_PageTitle],
                Description = localizer[FogResource.CampaignRegion_Meta_Description],
                Keywords = localizer[FogResource.CampaignRegion_Meta_Keywords],
                Title = localizer[FogResource.Navigation_Campaign],
                CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_CAMPAIGN_PATH
            };
        }

        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.BASE_CITY_PLANNER_PATH))
        {
            return new PageMetadata
            {
                PageTitle = localizer[FogResource.CityPlanner_PageTitle],
                Description = localizer[FogResource.CityPlanner_Meta_Description],
                Keywords = localizer[FogResource.CityPlanner_Meta_Keywords],
                Title = localizer[FogResource.CityPlanner_Title],
                CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_CITY_PLANNER_PATH,
                HelpPagePath = FogUrlBuilder.PageRoutes.HELP_CITY_PLANNER_SNAPSHOTS_PATH
            };
        }

        // heroes
        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.BASE_HEROES_PATH))
        {
            if (currentPageAbsolutePath == FogUrlBuilder.PageRoutes.BASE_HEROES_PATH)
            {
                return new PageMetadata
                {
                    PageTitle = localizer[FogResource.Heroes_PageTitle],
                    Description = localizer[FogResource.Heroes_Meta_Description],
                    Keywords = localizer[FogResource.Heroes_Meta_Keywords],
                    Title = localizer[FogResource.Hoh_Heroes],
                    CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_HEROES_PATH
                };
            }

            return new PageMetadata
            {
                PageTitle = localizer[FogResource.Hero_PageTitle, FogResource.Hoh_Heroes],
                Description = localizer[FogResource.Hero_Meta_Description],
                Keywords = localizer[FogResource.Hero_Meta_Keywords],
                Title = localizer[FogResource.Hoh_Heroes],
                CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_HEROES_PATH
            };
        }


        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.SUPPORT_US_PATH))
        {
            return new PageMetadata
            {
                PageTitle = localizer[FogResource.SupportUs_PageTitle],
                Description = localizer[FogResource.SupportUs_Meta_Description],
                Keywords = localizer[FogResource.SupportUs_Meta_Keywords],
                Title = localizer[FogResource.SupportUs_Title],
                CurrentHomePath = FogUrlBuilder.PageRoutes.SUPPORT_US_PATH
            };
        }

        // tools
        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.BASE_TOOLS_PATH))
        {
            if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.WONDER_COST_CALCULATOR_PATH))
            {
                return new PageMetadata
                {
                    PageTitle = localizer[FogResource.WonderCostCalculator_PageTitle],
                    Description = localizer[FogResource.WonderCostCalculator_Meta_Description],
                    Keywords = localizer[FogResource.WonderCostCalculator_Meta_Keywords],
                    Title = localizer[FogResource.Navigation_Tools],
                    CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_TOOLS_PATH
                };
            }

            if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.RESEARCH_CALCULATOR_PATH))
            {
                return new PageMetadata
                {
                    PageTitle = localizer[FogResource.ResearchCalculator_PageTitle],
                    Description = localizer[FogResource.ResearchCalculator_Meta_Description],
                    Keywords = localizer[FogResource.ResearchCalculator_Meta_Keywords],
                    Title = localizer[FogResource.Navigation_Tools],
                    CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_TOOLS_PATH
                };
            }

            if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.BUILDING_COST_CALCULATOR_PATH))
            {
                return new PageMetadata
                {
                    PageTitle = localizer[FogResource.BuildingCostCalculator_PageTitle],
                    Description = localizer[FogResource.BuildingCostCalculator_Meta_Description],
                    Keywords = localizer[FogResource.BuildingCostCalculator_Meta_Keywords],
                    Title = localizer[FogResource.Navigation_Tools],
                    CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_TOOLS_PATH
                };
            }


            return new PageMetadata
            {
                PageTitle = localizer[FogResource.Tools_PageTitle],
                Description = localizer[FogResource.Tools_Meta_Description],
                Keywords = localizer[FogResource.Tools_Meta_Keywords],
                Title = localizer[FogResource.Navigation_Tools],
                CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_TOOLS_PATH
            };
        }


        // treasure hunt
        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.BASE_TREASURE_HUNT_PATH))
        {
            if (currentPageAbsolutePath == FogUrlBuilder.PageRoutes.BASE_TREASURE_HUNT_PATH)
            {
                return new PageMetadata
                {
                    PageTitle = localizer[FogResource.TreasureHunt_PageTitle],
                    Description = localizer[FogResource.TreasureHunt_Meta_Description],
                    Keywords = localizer[FogResource.TreasureHunt_Meta_Keywords],
                    Title = localizer[FogResource.Navigation_TreasureHunt],
                    CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_TREASURE_HUNT_PATH
                };
            }

            return new PageMetadata
            {
                PageTitle = localizer[FogResource.TreasureHuntStage_PageTitle],
                Description = localizer[FogResource.TreasureHuntStage_Meta_Description],
                Keywords = localizer[FogResource.TreasureHuntStage_Meta_Keywords],
                Title = localizer[FogResource.Navigation_TreasureHunt],
                CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_TREASURE_HUNT_PATH
            };
        }


        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.BASE_WONDERS_PATH))
        {
            if (currentPageAbsolutePath == FogUrlBuilder.PageRoutes.BASE_WONDERS_PATH)
            {
                return new PageMetadata
                {
                    PageTitle = localizer[FogResource.Wonders_PageTitle],
                    Description = localizer[FogResource.Wonders_Meta_Description],
                    Keywords = localizer[FogResource.Wonders_Meta_Keywords],
                    Title = localizer[FogResource.Navigation_Wonders],
                    CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_WONDERS_PATH
                };
            }

            return new PageMetadata
            {
                PageTitle = localizer[FogResource.Wonder_PageTitle],
                Description = localizer[FogResource.Wonder_Meta_Description],
                Keywords = localizer[FogResource.Wonder_Meta_Keywords],
                Title = localizer[FogResource.Navigation_Wonders],
                CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_WONDERS_PATH
            };
        }

        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.BASE_ABOUT_PATH))
        {
            return new PageMetadata
            {
                PageTitle = localizer[FogResource.About_PageTitle],
                Description = localizer[FogResource.About_Meta_Description],
                Keywords = localizer[FogResource.About_Meta_Keywords],
                Title = localizer[FogResource.Navigation_About],
                CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_ABOUT_PATH
            };
        }

        // help
        if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.BASE_HELP_PATH))
        {
            if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.HELP_COMMAND_CENTER_PATH))
            {
                return new PageMetadata
                {
                    PageTitle = localizer[FogResource.Help_CommandCenter_PageTitle],
                    Description = localizer[FogResource.Help_CommandCenter_Meta_Description],
                    Keywords = localizer[FogResource.Help_CommandCenter_Meta_Keywords],
                    Title = localizer[FogResource.Navigation_Help],
                    CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_HELP_PATH
                };
            }

            if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.HELP_CITY_PLANNER_SNAPSHOTS_PATH))
            {
                return new PageMetadata
                {
                    PageTitle = localizer[FogResource.Help_CityPlannerSnapshots_PageTitle],
                    Description = localizer[FogResource.Help_CityPlannerSnapshots_Meta_Description],
                    Keywords = localizer[FogResource.Help_CityPlannerSnapshots_Meta_Keywords],
                    Title = localizer[FogResource.Navigation_Help],
                    CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_HELP_PATH
                };
            }

            if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.HELP_IMPORTING_IN_GAME_DATA_PATH))
            {
                return new PageMetadata
                {
                    PageTitle = localizer[FogResource.Help_ImportingInGameData_PageTitle],
                    Description = localizer[FogResource.Help_ImportingInGameData_Meta_Description],
                    Keywords = localizer[FogResource.Help_ImportingInGameData_Meta_Keywords],
                    Title = localizer[FogResource.Navigation_Help],
                    CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_HELP_PATH
                };
            }

            if (currentPageAbsolutePath.StartsWith(FogUrlBuilder.PageRoutes.HELP_BROWSER_EXTENSION_PATH))
            {
                return new PageMetadata
                {
                    PageTitle = localizer[FogResource.Help_BrowserExtension_PageTitle],
                    Description = localizer[FogResource.Help_BrowserExtension_Meta_Description],
                    Keywords = localizer[FogResource.Help_BrowserExtension_Meta_Keywords],
                    Title = localizer[FogResource.Navigation_Help],
                    CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_HELP_PATH
                };
            }

            return new PageMetadata
            {
                PageTitle = localizer[FogResource.Help_PageTitle],
                Description = localizer[FogResource.Help_Meta_Description],
                Keywords = localizer[FogResource.Help_Meta_Keywords],
                Title = localizer[FogResource.Navigation_Help],
                CurrentHomePath = FogUrlBuilder.PageRoutes.BASE_HELP_PATH
            };
        }

        // default
        return new PageMetadata
        {
            PageTitle = localizer[FogResource.Home_PageTitle],
            Description = localizer[FogResource.Home_Meta_Description],
            Keywords = localizer[FogResource.Home_Meta_Keywords],
            Title = localizer[FogResource.BrandName],
            CurrentHomePath = "/"
        };
    }
}
using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.Analytics.Pages;
using Ingweland.Fog.Application.Client.Web.Caching;
using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.Calculators;
using Ingweland.Fog.Application.Client.Web.Calculators.Interfaces;
using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Snapshots;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Snapshots.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.CommandCenter;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Factories;
using Ingweland.Fog.Application.Client.Web.Factories;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Migrations.CommandCenter;
using Ingweland.Fog.Application.Client.Web.Migrations.CommandCenter.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Factories;
using Ingweland.Fog.Application.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Ingweland.Fog.Application.Client.Web;

public static class DependencyInjection
{
    public static void AddWebAppApplicationServices(this IServiceCollection services)
    {
        services.AddApplicationCoreServices();

        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddMemoryCache(options => { options.ExpirationScanFrequency = TimeSpan.FromMinutes(5); });

        services.AddSingleton<IHeroProgressionCalculators, HeroProgressionCalculators>();

        services.AddSingleton<ICityMapEntityStyle, DefaultCityMapEntityStyle>();
        services.AddSingleton<IHohHeroLevelSpecsProvider, HohHeroLevelSpecsProvider>();
        services.AddSingleton<IBuildingLevelRangesFactory, BuildingLevelRangesFactory>();
        services.AddSingleton<IUnitStatFactory, UnitStatFactory>();
        services.AddSingleton<IHeroSupportUnitProfileFactory, HeroSupportUnitProfileFactory>();
        services.AddSingleton<IHohHeroProfileFactory, HeroProfileFactory>();
        services.AddSingleton<IHeroProfileIdentifierFactory, HeroProfileIdentifierFactory>();
        services.AddSingleton<IHohCommandCenterTeamFactory, CcProfileTeamFactory>();
        services.AddSingleton<ICcProfileTeamViewModelFactory, CcProfileTeamViewModelFactory>();
        services.AddSingleton<ICcProfileViewModelFactory, CcProfileViewModelFactory>();
        services.AddSingleton<IBarracksViewModelFactory, BarracksViewModelFactory>();

        services.AddScoped<IAssetUrlProvider, AssetUrlProvider>();
        services.AddScoped<IHohResourceIconUrlProvider, HohResourceIconUrlProvider>();
        services.AddScoped<IBattleWaveSquadViewModelFactory, BattleWaveSquadViewModelFactory>();
        services.AddScoped<IContinentBasicViewModelFactory, ContinentBasicViewModelFactory>();
        services.AddScoped<IHohTreasureHuntDifficultyIconUrlProvider, HohTreasureHuntDifficultyIconUrlProvider>();
        services.AddScoped<ITreasureHuntStageViewModelFactory, TreasureHuntStageViewModelFactory>();
        services.AddScoped<IHeroAbilityViewModelFactory, HeroAbilityViewModelFactory>();
        services.AddScoped<IWonderViewModelViewModelFactory, WonderViewModelViewModelFactory>();
        services.AddScoped<IHeroSupportUnitViewModelFactory, HeroSupportUnitViewModelFactory>();
        services.AddScoped<ICityUiService, CityUiService>();
        services.AddScoped<IBuildingMultilevelCostViewModelFactory, BuildingMultilevelCostViewModelFactory>();
        services.AddScoped<IToolsUiService, ToolsUiService>();
        services.AddScoped<IAgeTechnologiesFactory, AgeTechnologiesFactory>();
        services.AddScoped<IResearchCalculatorService, ResearchCalculatorService>();
        services.AddScoped<IBuildingSelectorTypesViewModelFactory, BuildingSelectorTypesViewModelFactory>();
        services.AddScoped<ICityPlanner, CityPlanner.CityPlanner>();
        services.AddScoped<IMapTransformationComponent, MapTransformationComponent>();
        services.AddScoped<ICityPlannerInteractionManager, CityPlannerInteractionManager>();
        services.AddScoped<ICityViewerInteractionManager, CityViewerInteractionManager>();
        services.AddScoped<IMapGrid, MapGrid>();
        services.AddScoped<ICityMapEntityInteractionComponent, CityMapEntityInteractionComponent>();
        services.AddScoped<ICommandManager, CommandManager>();
        services.AddScoped<ICityPlannerCommandFactory, CityPlannerCommandFactory>();
        services.AddScoped<IBuildingViewModelFactory, BuildingViewModelFactory>();
        services.AddScoped<IHohStorageIconUrlProvider, HohStorageIconUrlProvider>();
        services.AddScoped<ICityMapEntityViewModelFactory, CityMapEntityViewModelFactory>();
        services.AddScoped<IHohCityFactory, HohCityFactory>();
        services.AddScoped<ICityMapStateFactory, CityMapStateFactory>();
        services.AddScoped<IMapAreaRendererFactory, MapAreaRendererFactory>();
        services.AddScoped<ICityPlannerCityPropertiesViewModelFactory, CityPlannerCityPropertiesViewModelFactory>();
        services.AddScoped<IHappinessStatsViewModelFactory, HappinessStatsViewModelFactory>();
        services.AddScoped<IProductionStatsViewModelFactory, ProductionStatsViewModelFactory>();
        services.AddScoped<IAreaStatsViewModelFactory, AreaStatsViewModelFactory>();
        services.AddScoped<IBuildingTypeIconUrlProvider, BuildingTypeIconUrlProvider>();
        services.AddScoped<ICommandCenterUiService, CommandCenterUiService>();
        services.AddScoped<IHohHeroProfileViewModelFactory, HeroProfileViewModelFactory>();
        services.AddScoped<IHohCommandCenterProfileFactory, CcProfileFactory>();
        services.AddScoped<ICcProfileUiService, CcProfileUiService>();
        services.AddScoped<IHeroProfileUiService, HeroProfileUiService>();
        services.AddScoped<ISnapshotsComparisonViewModelFactory, SnapshotsComparisonViewModelFactory>();
        services.AddScoped<IHohPlayerAvatarUrlProvider, HohPlayerAvatarUrlProvider>();
        services.AddScoped<IAllianceAvatarUrlProvider, AllianceAvatarUrlProvider>();
        services.AddScoped<IStatsHubViewModelsFactory, StatsHubViewModelsFactory>();
        services.AddScoped<IStatsHubUiService, StatsHubUiService>();
        services.AddScoped<ICampaignUiService, CampaignUiService>();
        services.AddScoped<ITreasureHuntUiService, TreasureHuntUiService>();
        services.AddScoped<ICityUiService, CityUiService>();
        services.AddScoped<ICampaignDifficultyIconUrlProvider, CampaignDifficultyIconUrlProvider>();
        services.AddScoped<IEquipmentUiService, EquipmentUiService>();
        services.AddScoped<IEquipmentSlotTypeIconUrlProvider, EquipmentSlotTypeIconUrlProvider>();
        services.AddScoped<IWorkerIconUrlProvider, WorkerIconUrlProvider>();
        services.AddScoped<ICityMapBuildingGroupViewModelFactory, CityMapBuildingGroupViewModelFactory>();
        services.AddScoped<IBattleSearchRequestFactory, BattleSearchRequestFactory>();
        services.AddScoped<IBattleLogFactories, BattleLogFactories>();
        services.AddScoped<IBattleStatsViewModelFactory, BattleStatsViewModelFactory>();
        services.AddScoped<IResourceLocalizationService, ResourceLocalizationService>();
        services.AddScoped<ICommonUiService, CommonUiService>();
        services.AddScoped<ICityInspirationsUiService, CityInspirationsUiService>();
        services.AddScoped<IPlayerCitySnapshotViewModelFactory, PlayerCitySnapshotViewModelFactory>();
        services.AddScoped<ICityPlannerUiService, CityPlannerUiService>();
        services.AddScoped<IHohCoreDataCache, HohCoreDataCache>();
        services.AddScoped<ICcMigrationManager, CcMigrationManager>();
        services.AddScoped<ITopHeroesUiService, TopHeroesUiService>();
        services.AddScoped<IAnalyticsService, GoogleAnalyticsService>();
        services.AddScoped<IPlayerProfilePageAnalyticsService, PlayerProfilePageAnalyticsService>();
        services.AddScoped<IHeroComponentAnalyticsService, HeroComponentAnalyticsService>();
        services.AddScoped<IBattleLogPageAnalyticsService, BattleLogPageAnalyticsService>();
        services.AddScoped<IInspirationsPageAnalyticsService, InspirationsPageAnalyticsService>();
        services.AddScoped<IBuildingLevelSpecsFactory, BuildingLevelSpecsFactory>();
        services.AddScoped<IHeroRelicViewModelFactory, HeroRelicViewModelFactory>();
        services.AddScoped<IBattleViewModelFactory, BattleViewModelFactory>();
        services.AddScoped<IBattleUiService, BattleUiService>();
        services.AddScoped<IHohCoreDataViewModelsCache, HohCoreDataViewModelsCache>();
        services.AddScoped<IAllianceMemberRoleIconUrlProvider, AllianceMemberRoleIconUrlProvider>();
        services.AddScoped<IAlliancePageAnalyticsService, AlliancePageAnalyticsService>();
        services.AddScoped<IEquipmentInsightsViewModelFactory, EquipmentInsightsViewModelFactory>();
        services.AddScoped<IRelicInsightsViewModelFactory, RelicInsightsViewModelFactory>();
        services.AddScoped<IRelicUiService, RelicUiService>();

        services.AddScoped<CityPlannerSettings>();

        services.AddHttpClient<IBuildingRenderer, BuildingRenderer>();
    }
}

using Ingweland.Fog.Application.Client.Web.Calculators;
using Ingweland.Fog.Application.Client.Web.Calculators.Interfaces;
using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Application.Client.Web.CommandCenter;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Factories;
using Ingweland.Fog.Application.Client.Web.Factories;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Core.Calculators;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Application.Core.Formatters;
using Ingweland.Fog.Application.Core.Formatters.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Ingweland.Fog.Application.Client.Web;

public static class DependencyInjection
{
    public static void AddWebAppApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddMemoryCache();

        services.AddSingleton<IHeroProgressionCalculators, HeroProgressionCalculators>();
        services.AddSingleton<ICityCalculators, CityCalculators>();
        services.AddSingleton<ITimeFormatters, TimeFormatters>();
        services.AddSingleton<IBuildingTypeCssIconClassProvider, BuildingTypeCssIconClassProvider>();
        services.AddSingleton<ICityMapEntityStyle, DefaultCityMapEntityStyle>();
        services.AddSingleton<IHohHeroLevelSpecsProvider, HohHeroLevelSpecsProvider>();
        services.AddSingleton<IBuildingLevelRangesFactory, BuildingLevelRangesFactory>();
        services.AddSingleton<IUnitStatFactory, UnitStatFactory>();
        services.AddSingleton<IUnitStatCalculators, UnitStatCalculators>();
        services.AddSingleton<IUnitPowerCalculator, UnitPowerCalculator>();
        services.AddSingleton<IHohHeroSupportUnitProfileFactory, HohHeroSupportUnitProfileFactory>();
        services.AddSingleton<IHohHeroProfileFactory, HeroProfileFactory>();
        services.AddSingleton<IHohHeroProfileDtoFactory, HeroProfileDtoFactory>();
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
        services.AddScoped<IHeroBuilderViewModelFactory, HeroBuilderViewModelFactory>();
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
        services.AddScoped<IMapGrid, MapGrid>();
        services.AddScoped<ICityMapEntityInteractionComponent, CityMapEntityInteractionComponent>();
        services.AddScoped<ICityMapEntityFactory, CityMapEntityFactory>();
        services.AddScoped<IMapAreaFactory, MapAreaFactory>();
        services.AddScoped<IBuildingRenderer, BuildingRenderer>();
        services.AddScoped<ICommandManager, CommandManager>();
        services.AddScoped<ICityPlannerCommandFactory, CityPlannerCommandFactory>();
        services.AddScoped<IBuildingViewModelFactory, BuildingViewModelFactory>();
        services.AddScoped<IHohStorageIconUrlProvider, HohStorageIconUrlProvider>();
        services.AddScoped<ICityMapEntityViewModelFactory, CityMapEntityViewModelFactory>();
        services.AddScoped<IHohCityFactory, HohCityFactory>();
        services.AddScoped<ICityMapStateFactory, CityMapStateFactory>();
        services.AddScoped<IMapAreaRendererFactory, MapAreaRendererFactory>();
        services.AddScoped<ICityStatsProcessorFactory, CityStatsProcessorFactory>();
        services.AddScoped<ICityMapEntityStatsFactory, CityMapEntityStatsFactory>();
        services.AddScoped<IProductionStatsProcessorFactory, ProductionStatsProcessorFactory>();
        services.AddScoped<ICityPlannerCityPropertiesViewModelFactory, CityPlannerCityPropertiesViewModelFactory>();
        services.AddScoped<IHappinessStatsViewModelFactory, HappinessStatsViewModelFactory>();
        services.AddScoped<IProductionStatsViewModelFactory, ProductionStatsViewModelFactory>();
        services.AddScoped<IAreaStatsViewModelFactory, AreaStatsViewModelFactory>();
        services.AddScoped<IBuildingTypeIconUrlProvider, BuildingTypeIconUrlProvider>();
        services.AddScoped<ICommandCenterUiService, CommandCenterUiService>();
        services.AddScoped<IHohHeroProfileViewModelFactory, HeroProfileViewModelFactory>();
        services.AddScoped<IHohCommandCenterProfileFactory, CcProfileFactory>();
        services.AddScoped<ICcProfileUiService, CcProfileUiService>();
        services.AddScoped<ICcHeroesPlaygroundUiService, CcHeroesPlaygroundUiService>();
        services.AddScoped<CityPlannerSettings>();

        services.AddHttpClient<IWikipediaService, WikipediaService>()
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.BackoffType = DelayBackoffType.Exponential;
                options.Retry.MaxRetryAttempts = 3;
            });
    }
}

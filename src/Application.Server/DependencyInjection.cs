using Ingweland.Fog.Application.Core.Calculators;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Application.Core.Factories;
using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Services;
using Ingweland.Fog.Application.Core.Services.Hoh;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Services;
using Ingweland.Fog.Application.Server.Services.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;

namespace Ingweland.Fog.Application.Server;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly); });

        services.AddScoped<IUnitService, UnitService>();
        services.AddScoped<ICampaignService, CampaignService>();
        services.AddScoped<IHohGameLocalizationService, HohGameLocalizationService>();
        services.AddScoped<IHeroBasicDtoFactory, HeroBasicDtoFactory>();
        services.AddScoped<IUnitDtoFactory, UnitDtoFactory>();
        services.AddScoped<IHeroDtoFactory, HeroDtoFactory>();
        services.AddScoped<IRegionDtoFactory, RegionDtoFactory>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IBuildingTypeDtoFactory, BuildingTypeDtoFactory>();
        services.AddScoped<IBuildingGroupDtoFactory, BuildingGroupDtoFactory>();
        services.AddScoped<ITreasureHuntService, TreasureHuntService>();
        services.AddScoped<ITreasureHuntStageDtoFactory, TreasureHuntStageDtoFactory>();
        services.AddScoped<IHeroAbilityDtoFactory, HeroAbilityDtoFactory>();
        services.AddScoped<IWonderDtoFactory, WonderDtoFactory>();
        services.AddScoped<IUnitPowerCalculator, UnitPowerCalculator>();
        services.AddScoped<IResearchService, ResearchService>();
        services.AddScoped<ICommonService, CommonService>();
        services.AddScoped<ICityPlannerDataFactory, CityPlannerDataFactory>();
        services.AddScoped<IHohCityFactory, HohCityFactory>();
        services.AddScoped<IInGameStartupDataProcessingService, InGameStartupDataProcessingService>();
        services.AddScoped<ICommandCenterService, CommandCenterService>();
        services.AddScoped<IBarracksProfileFactory, BarracksProfileFactory>();
        services.AddScoped<ICommandCenterProfileFactory, CommandCenterProfileFactory>();
        services.AddScoped<IPlayerRankingService, PlayerRankingService>();
        services.AddScoped<IPlayerWithRankingsFactory, PlayerWithRankingsFactory>();
        services.AddScoped<IAllianceWithRankingsFactory, AllianceWithRankingsFactory>();
        services.AddScoped<IStatsHubService, StatsHubService>();
        services.AddScoped<IAllianceRankingService, AllianceRankingService>();
        services.TryAddScoped<IHohCitySnapshotFactory, HohCitySnapshotFactory>();

        services.AddHttpClient<IWikipediaService, WikipediaService>()
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.BackoffType = DelayBackoffType.Exponential;
                options.Retry.MaxRetryAttempts = 3;
            });
        
        return services;
    }
}

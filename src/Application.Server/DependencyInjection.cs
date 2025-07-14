using Ingweland.Fog.Application.Core;
using Ingweland.Fog.Application.Core.Factories;
using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Services;
using Ingweland.Fog.Application.Core.Services.Hoh;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Behaviors;
using Ingweland.Fog.Application.Server.Factories;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.PlayerCity;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services;
using Ingweland.Fog.Application.Server.Services.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Ingweland.Fog.Application.Server;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddApplicationCoreServices();

        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        
        services.AddMemoryCache(options =>
        {
            options.ExpirationScanFrequency = TimeSpan.FromMinutes(5);
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        });

        services.AddSingleton<InGameRawDataTablePartitionKeyProvider>();
        services.AddSingleton<InGameBinDataTablePartitionKeyProvider>();
        services.AddSingleton<IGameWorldsProvider, GameWorldsProvider>();
        services.AddSingleton<IFailedPlayerCityFetchesCache, FailedPlayerCityFetchesCache>();

       
        services.AddScoped<IHohCityFactory, HohCityFactory>();
        services.AddScoped<IInGameStartupDataProcessingService, InGameStartupDataProcessingService>();
       
        services.AddScoped<IBarracksProfileFactory, BarracksProfileFactory>();
        services.AddScoped<ICommandCenterProfileFactory, CommandCenterProfileFactory>();
        services.AddScoped<IPlayerWithRankingsFactory, PlayerWithRankingsFactory>();
        services.AddScoped<IAllianceWithRankingsFactory, AllianceWithRankingsFactory>();
        services.AddScoped<IStatsHubService, StatsHubService>();
        services.AddScoped<IBattleDefinitionIdFactory, BattleDefinitionIdFactory>();
        services.AddScoped<IBattleService, BattleService>();
        services.AddScoped<IBattleSearchResultFactory, BattleSearchResultFactory>();
        services.AddScoped<IBattleStatsDtoFactory, BattleStatsDtoFactory>();
        services.AddScoped<IBattleQueryService, BattleQueryService>();
        services.AddScoped<IUnitBattleDtoFactory, UnitBattleDtoFactory>();
        services.AddScoped<IInGameDataParsingService, InGameDataParsingService>();
        services.AddScoped<IPlayerCityService, PlayerCityService>();
        services.AddScoped<ICityPlannerService, CityPlannerService>();
        services.AddScoped<IHohCityCreationService, HohCityCreationService>();

        services.AddHttpClient<IWikipediaService, WikipediaService>()
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.BackoffType = DelayBackoffType.Exponential;
                options.Retry.MaxRetryAttempts = 3;
            });

        return services;
    }
}

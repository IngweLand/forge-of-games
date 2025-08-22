using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.Functions.Services;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Orchestration;
using Ingweland.Fog.Functions.Validators;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BattleService = Ingweland.Fog.Functions.Services.BattleService;

namespace Ingweland.Fog.Functions;

public static class DependencyInjection
{
    public static IServiceCollection AddFunctionsServices(this IServiceCollection services)
    {
        services.AddSingleton<EndpointValidator>();
        services.AddSingleton<HohHelperResponseDtoValidator>();
        services.AddSingleton<PayloadValidator>();
        services.AddSingleton<WorldValidator>();
        services.AddSingleton<SubmissionIdValidator>();

        services.AddTransient<DatabaseWarmUpService>();

        services.AddScoped<IPvpRankingService, PvpRankingService>();
        services.AddScoped<IPlayerRankingService, PlayerRankingService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IPlayerAgeHistoryService, PlayerAgeHistoryService>();
        services.AddScoped<IPlayerNameHistoryService, PlayerNameHistoryService>();
        services.AddScoped<IAllianceRankingService, AllianceRankingService>();
        services.AddScoped<IAllianceService, AllianceService>();
        services.AddScoped<IAllianceNameHistoryService, AllianceNameHistoryService>();
        services.AddScoped<IAllianceMembersService, AllianceMembersService>();
        services.AddScoped<IPvpBattleService, PvpBattleService>();
        services.AddScoped<IBattleService, BattleService>();
        services.AddScoped<IBattleStatsService, BattleStatsService>();
        services.AddScoped<IPvpBattlesBulkUpdater, PvpBattlesBulkUpdater>();
        services.AddScoped<IPlayersUpdateManager, PlayersUpdateManager>();
        services.AddScoped<ITopPlayersUpdateManager, TopPlayersUpdateManager>();
        services.AddScoped<IPlayerCityFetcher, PlayerCityFetcher>();
        services.AddScoped<ITopPlayersCityFetcher, TopPlayersCityFetcher>();
        services.AddScoped<ITopAllianceMemberProfilesUpdateManager, TopAllianceMemberProfilesUpdateManager>();
        services.AddScoped<ICultureUsageRatioUpdater, CultureUsageRatioUpdater>();
        services.AddScoped<IPlayerUpdater, PlayerUpdater>();
        services.AddScoped<ITopAllianceMemberUpdateManager, TopAllianceMemberUpdateManager>();
        services.AddScoped<ITopHeroInsightsProcessor, TopHeroInsightsProcessor>();
        services.AddScoped<IAllianceMembersUpdateManager, AllianceMembersUpdateManager>();
        services.AddScoped<IBattleTimelineService, BattleTimelineService>();

        services.AddScoped<HohHelperResponseDtoToTablePkConverter>();

        return services;
    }

    public static void AddConfigurations(this FunctionsApplicationBuilder builder)
    {
        builder.Configuration.AddUserSecrets<Program>(optional: true);
        builder.Services.Configure<HohServerCredentials>(
            builder.Configuration.GetSection(HohServerCredentials.CONFIGURATION_PROPERTY_NAME));
        builder.Services.Configure<StorageSettings>(
            builder.Configuration.GetSection(StorageSettings.CONFIGURATION_PROPERTY_NAME));
        builder.Services.Configure<ResourceSettings>(
            builder.Configuration.GetSection(ResourceSettings.CONFIGURATION_PROPERTY_NAME));
    }
}

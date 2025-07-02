using Ingweland.Fog.Functions.Services;
using Ingweland.Fog.Functions.Validators;
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

        services.AddTransient<DatabaseWarmUpService>();

        services.AddScoped<IPvpRankingService, PvpRankingService>();
        services.AddScoped<IPlayerRankingService, PlayerRankingService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IPlayerAgeHistoryService, PlayerAgeHistoryService>();
        services.AddScoped<IPlayerNameHistoryService, PlayerNameHistoryService>();
        services.AddScoped<IPlayerAllianceNameHistoryService, PlayerAllianceNameHistoryService>();
        services.AddScoped<IAllianceRankingService, AllianceRankingService>();
        services.AddScoped<IAllianceService, AllianceService>();
        services.AddScoped<IAllianceNameHistoryService, AllianceNameHistoryService>();
        services.AddScoped<IAllianceMembersService, AllianceMembersService>();
        services.AddScoped<IPvpBattleService, PvpBattleService>();
        services.AddScoped<IBattleService, BattleService>();
        services.AddScoped<IBattleStatsService, BattleStatsService>();
        services.AddScoped<HohHelperResponseDtoToTablePkConverter>();

        return services;
    }
}

using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Functions.Services;
using Ingweland.Fog.Functions.Validators;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Ingweland.Fog.Functions;

public static class DependencyInjection
{
    public static IServiceCollection AddFunctionsServices(this IServiceCollection services)
    {
        services.AddSingleton<IGameWorldsProvider, GameWorldsProvider>();
        services.AddSingleton<EndpointValidator>();
        services.AddSingleton<HohHelperResponseDtoValidator>();
        services.AddSingleton<PayloadValidator>();
        services.AddSingleton<WorldValidator>();
        services.AddSingleton<InGameRawDataTablePartitionKeyProvider>();
        
        services.AddTransient<DatabaseWarmUpService>();
        
        services.AddScoped<IInGameDataParsingService, InGameDataParsingService>();
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
        services.AddScoped<HohHelperResponseDtoToTablePkConverter>();

        return services;
    }
}
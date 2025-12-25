using Ingweland.Fog.Application.Core.Calculators;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Application.Core.Factories;
using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Formatters;
using Ingweland.Fog.Application.Core.Formatters.Interfaces;
using Ingweland.Fog.Application.Core.Interfaces;
using Ingweland.Fog.Application.Core.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Ingweland.Fog.Application.Core;

public static class DependencyInjection
{
    public static void AddApplicationCoreServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddSingleton<ICityCalculators, CityCalculators>();
        services.AddSingleton<ITimeFormatters, TimeFormatters>();
        services.AddSingleton<ICityExpansionsHasher, CityExpansionsHasher>();

        services.AddSingleton<IUnitStatCalculators, UnitStatCalculators>();
        services.AddSingleton<IUnitPowerCalculator, UnitPowerCalculator>();
        services.AddSingleton<InitCityConfigs>();

        services.AddScoped<ICityPlannerDataService, CityPlannerDataService>();
        services.AddScoped<ICityMapStateCoreFactory, CityMapStateCoreFactory>();
        services.AddScoped<ICityStatsProcessorFactory, CityStatsProcessorFactory>();
        services.AddScoped<ICityStatsCalculator, CityStatsCalculator>();
        services.AddScoped<IHohCitySnapshotFactory, HohCitySnapshotFactory>();
        services.AddScoped<ICityMapEntityFactory, CityMapEntityFactory>();
        services.AddScoped<IMapAreaFactory, MapAreaFactory>();
        services.AddScoped<IProductionStatsProcessorFactory, ProductionStatsProcessorFactory>();
        services.AddScoped<ICityMapEntityStatsFactory, CityMapEntityStatsFactory>();
        services.AddScoped<IBattleDefinitionIdFactory, BattleDefinitionIdFactory>();
        services.AddScoped<IHohCityFactory, HohCityFactory>();
    }
}

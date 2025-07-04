using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Application.Core.Factories;
using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Ingweland.Fog.Application.Core;

public static class DependencyInjection
{
    public static void AddApplicationCoreServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        
        services.AddScoped<ICityPlannerDataService, CityPlannerDataService>();
        services.AddScoped<ICityMapStateCoreFactory, CityMapStateCoreFactory>();
        services.AddScoped<ICityStatsProcessorFactory, CityStatsProcessorFactory>();
        services.AddScoped<ICityStatsCalculator, CityStatsCalculator>();
        services.AddScoped<IHohCitySnapshotFactory, HohCitySnapshotFactory>();
        services.AddScoped<ICityMapEntityFactory, CityMapEntityFactory>();
        services.AddScoped<IMapAreaFactory, MapAreaFactory>();
        services.AddScoped<IProductionStatsProcessorFactory, ProductionStatsProcessorFactory>();
    }
}

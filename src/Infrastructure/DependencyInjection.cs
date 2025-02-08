using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh;
using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.Repositories;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        
        services.AddSingleton<IHohCoreDataRepository, HohCoreDataRepository>();
        services.AddSingleton<IHohGameLocalizationDataRepository, HohGameLocalizationDataRepository>();
        services.TryAddSingleton<IHohDataProvider, DefaultHohDataProvider>();
        services.AddScoped<IInGameStartupDataRepository, InGameStartupDataRepository>();
        services.AddScoped<ICommandCenterProfileRepository, CommandCenterProfileRepository>();
        services.AddScoped<IHohCityRepository, HohCityRepository>();
    }

    public static IServiceCollection AddTableStorage(this IServiceCollection services)
    {
        services.AddSingleton<ITableStorageRepository<CcProfileTableEntity>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<StorageSettings>>().Value;
            return new TableStorageRepository<CcProfileTableEntity>(options.ConnectionString,
                options.CommandCenterProfilesTable);
        });
        
        services.AddSingleton<ITableStorageRepository<InGameStartupDataTableEntity>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<StorageSettings>>().Value;
            return new TableStorageRepository<InGameStartupDataTableEntity>(options.ConnectionString,
                options.HohStartupDataTable);
        });
        
        services.AddSingleton<ITableStorageRepository<HohCityTableEntity>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<StorageSettings>>().Value;
            return new TableStorageRepository<HohCityTableEntity>(options.ConnectionString,
                options.CityPlannerCitiesTable);
        });

        return services;
    }
}

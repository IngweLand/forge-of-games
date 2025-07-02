using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Settings;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.WebApp.Client.Models;
using Ingweland.Fog.WebApp.Client.Services;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Ingweland.Fog.WebApp.Services;

namespace Ingweland.Fog.WebApp;

internal static class DependencyInjection
{
    public static void AddWebAppServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        
        services.AddScoped<CityPlannerNavigationState>();
        services.AddScoped<IClientLocaleService, DummyClientLocaleService>();
        services.AddScoped<IPersistenceService, DummyPersistenceService>();
        services.AddScoped<IInGameStartupDataService, DummyInGameStartupDataService>();
        services.AddScoped<IJSInteropService, DummyJSInteropService>();
        services.AddScoped<ICityPlannerSharingService, DummyCityPlannerSharingService>();
        services.AddScoped<ICommandCenterProfileSharingService, DummyCommandCenterProfileSharingService>();
        services.AddScoped<ILocalStorageBackupService, DummyLocalStorageBackupService>();
        services.AddScoped<IMainMenuService, MainMenuService>();
        services.AddScoped<IPageMetadataService, PageMetadataService>();
    }

    public static void AddWebAppSettings(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<AssetsSettings>(
            builder.Configuration.GetSection(AssetsSettings.CONFIGURATION_PROPERTY_NAME));

        builder.Services.Configure<ResourceSettings>(
            builder.Configuration.GetSection(ResourceSettings.CONFIGURATION_PROPERTY_NAME));

        builder.Services.Configure<StorageSettings>(
            builder.Configuration.GetSection(StorageSettings.CONFIGURATION_PROPERTY_NAME));
        
        builder.Services.Configure<HohServerCredentials>(
            builder.Configuration.GetSection(HohServerCredentials.CONFIGURATION_PROPERTY_NAME));
    }
}

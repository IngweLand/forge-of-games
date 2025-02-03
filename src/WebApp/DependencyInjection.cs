using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.Settings;
using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.WebApp.Services;
using Ingweland.Fog.WebApp.Services.Abstractions;

namespace Ingweland.Fog.WebApp;

internal static class DependencyInjection
{
    public static void AddWebAppServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        services.AddScoped<IUnitUiService, UnitUiService>();
        services.AddScoped<ICampaignUiServerService, CampaignUiServerService>();
        services.AddScoped<ICityUiService, CityUiService>();
        services.AddScoped<ITreasureHuntUiService, TreasureHuntUiServerService>();
        services.AddScoped<IClientLocaleService, DummyClientLocaleService>();
        services.AddScoped<IPersistenceService, DummyPersistenceService>();
        services.AddScoped<IInGameStartupDataService, DummyInGameStartupDataService>();
    }

    public static void AddWebAppSettings(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<AssetsSettings>(
            builder.Configuration.GetSection(AssetsSettings.CONFIGURATION_PROPERTY_NAME));

        builder.Services.Configure<ResourceSettings>(
            builder.Configuration.GetSection(ResourceSettings.CONFIGURATION_PROPERTY_NAME));

        builder.Services.Configure<StorageSettings>(
            builder.Configuration.GetSection(StorageSettings.CONFIGURATION_PROPERTY_NAME));
    }
}

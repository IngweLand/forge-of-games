using System.Text.Json;
using System.Text.Json.Serialization;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Settings;
using Ingweland.Fog.Application.Core.Services;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Shared.Helpers;
using Ingweland.Fog.WebApp.Client.Net;
using Ingweland.Fog.WebApp.Client.Services;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Polly;
using Refit;

namespace Ingweland.Fog.WebApp.Client;

internal static class DependencyInjection
{
    public static void AddWebAppClientServices(this IServiceCollection services, string baseAddress)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddScoped<CityPlannerNavigationState>();
        services.AddScoped<IClientLocaleService, ClientLocaleService>();
        services.AddScoped<IJSInteropService, JSInteropService>();
        services.AddScoped<IClipboardService, ClipboardService>();
        services.AddScoped<IPersistenceService, PersistenceService>();
        services.AddScoped<IMainMenuService, MainMenuService>();
        services.AddScoped<IPageMetadataService, PageMetadataService>();
        services.AddScoped<ILocalStorageBackupService, LocalStorageBackupService>();
        services.AddScoped<IEquipmentProfilePersistenceService, EquipmentProfilePersistenceService>();
        services.AddScoped<AppBarService>();
        services.AddScoped<CityStrategyNavigationState>();

        var refitSettings = new RefitSettings
        {
            ContentSerializer = new ProtobufHttpContentSerializer(new ProtobufSerializer()),
        };
        AddRefitProtobufApiClient<IUnitService>(services, baseAddress, refitSettings);
        AddRefitProtobufApiClient<ICityService>(services, baseAddress, refitSettings);
        AddRefitProtobufApiClient<ICommonService>(services, baseAddress, refitSettings);
        AddRefitProtobufApiClient<IResearchService>(services, baseAddress, refitSettings);
        AddRefitProtobufApiClient<ICommandCenterService>(services, baseAddress, refitSettings);
        AddRefitProtobufApiClient<ICampaignService>(services, baseAddress, refitSettings);
        AddRefitProtobufApiClient<ITreasureHuntService>(services, baseAddress, refitSettings);
        AddRefitProtobufApiClient<IRelicCoreDataService>(services, baseAddress, refitSettings);
        AddRefitProtobufApiClient<IPlayerCityStrategyService>(services, baseAddress, refitSettings);

        var refitJsonSettings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(GetDefaultJsonSerializerOptions()),
        };
        AddRefitJsonApiClient<ICommandCenterSharingService>(services, baseAddress, refitJsonSettings);
        AddRefitJsonApiClient<IInGameStartupDataService>(services, baseAddress, refitJsonSettings);
        AddRefitJsonApiClient<ICityPlannerSharingService>(services, baseAddress, refitJsonSettings);
        AddRefitJsonApiClient<IStatsHubService>(services, baseAddress, refitJsonSettings);
        AddRefitJsonApiClient<IWikipediaService>(services, baseAddress, refitJsonSettings);
        AddRefitJsonApiClient<IBattleService>(services, baseAddress, refitJsonSettings);
        AddRefitJsonApiClient<ICityPlannerService>(services, baseAddress, refitJsonSettings);
        AddRefitJsonApiClient<IEquipmentService>(services, baseAddress, refitJsonSettings);
        AddRefitJsonApiClient<IRelicService>(services, baseAddress, refitJsonSettings);
        AddRefitJsonApiClient<IFogCommonService>(services, baseAddress, refitJsonSettings);
        AddRefitJsonApiClient<IInGameEventService>(services, baseAddress, refitJsonSettings);
        AddRefitJsonApiClient<IFogSharingService>(services, baseAddress, refitJsonSettings, "api");
        AddRefitJsonApiClient<ISharedCityStrategyService>(services, baseAddress, refitJsonSettings, "api");
    }

    private static void AddRefitProtobufApiClient<T>(IServiceCollection services, string baseAddress,
        RefitSettings settings)
        where T : class
    {
        services
            .AddRefitClient<T>(settings)
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri($"{baseAddress}api/hoh");
                client.DefaultRequestHeaders.Add("Accept", "application/x-protobuf");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            })
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.BackoffType = DelayBackoffType.Exponential;
                options.Retry.MaxRetryAttempts = 3;
                options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(15);
            });
    }

    private static void AddRefitJsonApiClient<T>(IServiceCollection services, string baseAddress,
        RefitSettings settings, string group = "api/hoh")
        where T : class
    {
        services
            .AddRefitClient<T>(settings)
            .ConfigureHttpClient(client => { client.BaseAddress = new Uri($"{baseAddress}{group}"); })
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.BackoffType = DelayBackoffType.Exponential;
                options.Retry.MaxRetryAttempts = 3;
                options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(15);
            });
    }

    public static void AddWebAppClientSettings(this WebAssemblyHostBuilder builder)
    {
        builder.Services.Configure<AssetsSettings>(
            builder.Configuration.GetSection("AssetsSettings"));
    }

    private static JsonSerializerOptions GetDefaultJsonSerializerOptions()
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        };
        jsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());

        return jsonSerializerOptions;
    }
}

using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.Settings;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Shared.Helpers;
using Ingweland.Fog.WebApp.Client.Net;
using Ingweland.Fog.WebApp.Client.Services;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Ingweland.Fog.WebApp.Client.Utilities;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Polly;
using Refit;

namespace Ingweland.Fog.WebApp.Client;

internal static class DependencyInjection
{
    public static void AddWebAppClientServices(this IServiceCollection services, string baseAddress)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddSingleton<ITypeSafeMemoryCache, TypeSafeMemoryCache>();

        services.AddScoped<IUnitUiService, UnitUiService>();
        services.AddScoped<IHeroBuilderService, HeroBuilderService>();
        services.AddScoped<IClientLocaleService, ClientLocaleService>();
        services.AddScoped<IJSInteropService, JSInteropService>();
        services.AddScoped<IClipboardService, ClipboardService>();
        services.AddScoped<IPersistenceService, PersistenceService>();

        var refitSettings = new RefitSettings
        {
            ContentSerializer = new ProtobufHttpContentSerializer(new ProtobufSerializer()),
        };
        AddRefitProtobufApiClient<IUnitService>(services, baseAddress, refitSettings);
        AddRefitProtobufApiClient<ICityService>(services, baseAddress, refitSettings);
        AddRefitProtobufApiClient<ICommonService>(services, baseAddress, refitSettings);
        AddRefitProtobufApiClient<IResearchService>(services, baseAddress, refitSettings);
        AddRefitProtobufApiClient<ICommandCenterService>(services, baseAddress, refitSettings);
        AddRefitJsonApiClient<ICommandCenterProfileSharingService>(services, baseAddress);
        AddRefitJsonApiClient<IInGameStartupDataService>(services, baseAddress);
    }

    private static void AddRefitProtobufApiClient<T>(IServiceCollection services, string baseAddress, RefitSettings settings)
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
            });
    }
    
    private static void AddRefitJsonApiClient<T>(IServiceCollection services, string baseAddress)
        where T : class
    {
        services
            .AddRefitClient<T>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri($"{baseAddress}api/hoh");
            })
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.BackoffType = DelayBackoffType.Exponential;
                options.Retry.MaxRetryAttempts = 3;
            });
    }

    public static void AddWebAppClientSettings(this WebAssemblyHostBuilder builder)
    {
        builder.Services.Configure<AssetsSettings>(
            builder.Configuration.GetSection("AssetsSettings"));
    }
}

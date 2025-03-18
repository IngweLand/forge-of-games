using System.Net;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Factories;
using Ingweland.Fog.InnSdk.Hoh.Factories.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Net;
using Ingweland.Fog.InnSdk.Hoh.Net.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Services;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;

namespace Ingweland.Fog.InnSdk.Hoh;

public static class DependencyInjection
{
    public static void AddInnSdkServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        
        services.TryAddSingleton<IGameConnectionManager, GameConnectionManager>();
        services.TryAddScoped<IGameCredentialsManager, DefaultGameCredentialsManager>();

        services.AddScoped<IWebAuthenticationResponseHandler, WebAuthenticationResponseHandler>();
        services.AddScoped<IWebAuthPayloadFactory, WebAuthPayloadFactory>();
        services.AddScoped<ILocalizationRequestPayloadFactory, LocalizationRequestPayloadFactory>();
        services.AddScoped<IGameDesignRequestPayloadFactory, GameDesignRequestPayloadFactory>();
        services.AddScoped<IPlayerRankingRequestPayloadFactory, PlayerRankingRequestPayloadFactory>();
        services.AddScoped<IAllianceRankingRequestPayloadFactory, AllianceRankingRequestPayloadFactory>();
        services.AddScoped<IPvpRankingRequestPayloadFactory, PvpRankingRequestPayloadFactory>();
        
        services.AddScoped<IStaticDataService, StaticDataService>();
        services.AddScoped<IRankingsService, RankingsService>();
        services.AddScoped<IDataParsingService, DataParsingService>();

        services.AddScoped<Lazy<IStaticDataService>>(sp =>
            new Lazy<IStaticDataService>(sp.GetRequiredService<IStaticDataService>));
        services.AddScoped<Lazy<IRankingsService>>(sp =>
            new Lazy<IRankingsService>(sp.GetRequiredService<IRankingsService>));

        services.AddHttpClient<IWebAuthenticationService, WebAuthenticationService>()
            .AddSdkHttpClientResilienceHandler();

        services.AddHttpClient<IGameApiClient, GameApiClient>()
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip,
            })
            .AddSdkHttpClientResilienceHandler();

        services.AddScoped<IInnSdkClient, InnSdkClient>();
    }

    private static void AddSdkHttpClientResilienceHandler(this IHttpClientBuilder builder)
    {
        builder.AddStandardResilienceHandler(options =>
        {
            options.Retry.BackoffType = DelayBackoffType.Exponential;
            options.Retry.MaxRetryAttempts = 3;
        });
    }
}

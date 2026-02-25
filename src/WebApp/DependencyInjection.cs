using System.Security.Claims;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Settings;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.WebApp.Apis;
using Ingweland.Fog.WebApp.Client.Services;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Ingweland.Fog.WebApp.Services;
using Ingweland.Fog.WebApp.Startup;
using Ingweland.Fog.WebApp.Startup.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Ingweland.Fog.WebApp;

internal static class DependencyInjection
{
    public static void AddWebAppServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddSingleton<IStartupTask, PreloadDataStartupTask>();
        services.AddSingleton<IProtobufResponseFactory, ProtobufResponseFactory>();

        services.AddHostedService<StartupTaskHostedService>();

        services.AddScoped<IClientLocaleService, DummyClientLocaleService>();
        services.AddScoped<IPersistenceService, DummyPersistenceService>();
        services.AddScoped<IInGameStartupDataService, DummyInGameStartupDataService>();
        services.AddScoped<IJSInteropService, DummyJSInteropService>();
        services.AddScoped<ICityPlannerSharingService, DummyCityPlannerSharingService>();
        services.AddScoped<ICommandCenterSharingService, DummyCommandCenterSharingService>();
        services.AddScoped<ILocalStorageBackupService, DummyLocalStorageBackupService>();
        services.AddScoped<IFogSharingService, DummyFogSharingService>();
        services.AddScoped<IEquipmentProfilePersistenceService, DummyEquipmentProfilePersistenceService>();
        services.AddScoped<ISharedImageUploaderService, DummySharedImageUploaderService>();
        services.AddScoped<IAdSenseService, DummyAdSenseService>();

        services.AddScoped<CityPlannerNavigationState>();
        services.AddScoped<IMainMenuService, MainMenuService>();
        services.AddScoped<IPageMetadataService, PageMetadataService>();
        services.AddScoped<AppBarService>();
        services.AddScoped<CityStrategyNavigationState>();
    }

    public static void AddWebAppSettings(this IHostApplicationBuilder builder)
    {
        if (!builder.Environment.IsDevelopment())
        {
            var connectionString = builder.Configuration.GetConnectionString("AppConfiguration") ??
                throw new InvalidOperationException("The Connection string  `AppConfiguration` was not found.");

            builder.Configuration.AddAzureAppConfiguration(options =>
            {
                options.Connect(connectionString)
                    .Select($"{ResourceSettings.CONFIGURATION_PROPERTY_NAME}:*")
                    .Select("Logging:LogLevel:*")
                    .ConfigureRefresh(refreshOptions =>
                    {
                        refreshOptions
                            .Register("Logging:LogLevel:Sentinel", refreshAll: true)
                            .Register($"{ResourceSettings.CONFIGURATION_PROPERTY_NAME}:Sentinel", refreshAll: true)
                            .SetRefreshInterval(TimeSpan.FromMinutes(4));
                    });
            });

            builder.Services.AddAzureAppConfiguration();
        }

        builder.Services.Configure<AssetsSettings>(
            builder.Configuration.GetSection(AssetsSettings.CONFIGURATION_PROPERTY_NAME));

        builder.Services.Configure<ResourceSettings>(
            builder.Configuration.GetSection(ResourceSettings.CONFIGURATION_PROPERTY_NAME));

        builder.Services.Configure<StorageSettings>(
            builder.Configuration.GetSection(StorageSettings.CONFIGURATION_PROPERTY_NAME));

        builder.Services.Configure<HohServerCredentials>(
            builder.Configuration.GetSection(HohServerCredentials.CONFIGURATION_PROPERTY_NAME));

        builder.Services.Configure<MaintenanceModeSettings>(
            builder.Configuration.GetSection(MaintenanceModeSettings.CONFIGURATION_PROPERTY_NAME));
        builder.Services.Configure<PatreonSettings>(
            builder.Configuration.GetSection(PatreonSettings.CONFIGURATION_PROPERTY_NAME));
    }
    
    public static void AddPatreon(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services
            .AddAuthentication()
            .AddPatreon(options =>
            {
                options.ClientId = configuration["PatreonSettings:ClientId"];
                options.ClientSecret = configuration["PatreonSettings:ClientSecret"];
                options.CallbackPath = "/signin-patreon";
                options.Fields.Add("email");
                options.Includes.Add("memberships");

                options.Scope.Add("identity");
                options.Scope.Add("identity[email]");
                options.Scope.Add("identity.memberships");

                options.SaveTokens = true;

                options.Events.OnCreatingTicket = async ctx =>
                {
                    // parse membership JSON here
                    // extract patron_status / tier
                    // ctx.Identity.AddClaim(new Claim("PatronStatus", "active"));
                    // ctx.Identity.AddClaim(new Claim("TierAmount", "500"));
                    
                    var userId = ctx.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                    var patronStatus = ctx.Principal.FindFirstValue("patron_status") ?? "inactive";

                    // Create Identity user if not exists
                    var userManager = ctx.HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
                    var signInManager = ctx.HttpContext.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();

                    var user = await userManager.FindByLoginAsync("Patreon", userId);
                    if (user == null)
                    {
                        user = new IdentityUser { UserName = userId };
                        await userManager.CreateAsync(user);
                        await userManager.AddLoginAsync(user, new UserLoginInfo("Patreon", userId, "Patreon"));
                    }

                    // Sign in locally
                    await signInManager.SignInAsync(user, isPersistent: false);
                };
            });
        }
}

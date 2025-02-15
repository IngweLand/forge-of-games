using Ingweland.Fog.Application.Server;
using Ingweland.Fog.Application.Server.Services.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.Functions;
using Ingweland.Fog.Infrastructure;
using Ingweland.Fog.InnSdk.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>(optional: true);
builder.Services.Configure<HohServerCredentials>(
    builder.Configuration.GetSection(HohServerCredentials.CONFIGURATION_PROPERTY_NAME));
builder.Services.Configure<StorageSettings>(
    builder.Configuration.GetSection(StorageSettings.CONFIGURATION_PROPERTY_NAME));

builder.ConfigureFunctionsWebApplication();
builder.Services.AddInnSdkServices();
builder.Services.AddScoped<IGameWorldsProvider, GameWorldsProvider>();
builder.Services.AddInfrastructureServices();
builder.Services.AddInfrastructureDbContext(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddScoped<IInGameDataParsingService, InGameDataParsingService>();
// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();
builder.Services.AddLogging(logging =>
{
    logging.SetMinimumLevel(LogLevel.Information);
    logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
});
builder.Build().Run();

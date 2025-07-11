using Ingweland.Fog.Application.Server;
using Ingweland.Fog.Application.Server.Services.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.Functions;
using Ingweland.Fog.Functions.Services;
using Ingweland.Fog.Infrastructure;
using Ingweland.Fog.InnSdk.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Shared;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);
builder.AddConfigurations();

builder.ConfigureFunctionsWebApplication();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddInnSdkServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddInfrastructureDbContext(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSharedServices();
builder.Services.AddFunctionsServices();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();
var customEnvironment = builder.Configuration.GetValue<string>("CustomEnvironment");
if(customEnvironment is "Development" or "Staging")
{
    builder.Services.AddLogging(logging =>
    {
        logging.SetMinimumLevel(LogLevel.Information);
        logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Information);
        logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
    });
}
else
{
    builder.Services.AddLogging(logging =>
    {
        logging.SetMinimumLevel(LogLevel.Warning);
        logging.AddFilter("Ingweland", LogLevel.Information);
    });
}

builder.Build().Run();

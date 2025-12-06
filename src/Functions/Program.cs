using FluentResults;
using Ingweland.Fog.Application.Server;
using Ingweland.Fog.Functions;
using Ingweland.Fog.HohCoreDataParserSdk;
using Ingweland.Fog.Infrastructure;
using Ingweland.Fog.InnSdk.Hoh;
using Ingweland.Fog.Shared;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();
builder.AddConfigurations();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddInnSdkServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddInfrastructureDbContext(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSharedServices();
builder.Services.AddFunctionsServices();
builder.Services.AddHohCoreDataParserSdkServices();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();
var customEnvironment = builder.Configuration.GetValue<string>("CustomEnvironment");
if (customEnvironment is "Development" or "Staging")
{
    builder.Services.AddLogging(logging =>
    {
        logging.SetMinimumLevel(LogLevel.Information);
        logging.AddFilter("Polly", LogLevel.Warning);
        logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);
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

var app = builder.Build();

var resultLogger = app.Services.GetRequiredService<IResultLogger>();
Result.Setup(cfg => { cfg.Logger = resultLogger; });

app.Run();

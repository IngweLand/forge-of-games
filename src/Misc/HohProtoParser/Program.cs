using System.Net;
using Ingweland.Fog.HohProtoParser;
using Ingweland.Fog.InnSdk.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var services = new ServiceCollection();

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

services.Configure<HohServerCredentials>(
    configuration.GetSection(HohServerCredentials.CONFIGURATION_PROPERTY_NAME));

services.AddLogging(builder =>
{
    builder.ClearProviders();
    builder.AddSerilog();
});

services.AddSharedServices();
services.AddInnSdkServices();
services.AddAutoMapper(typeof(Program).Assembly);
services.AddTransient<LocalizationParser>();
services.AddTransient<GameDesignDataParser>();
services.AddTransient<IDownloader, Downloader>();

var serviceProvider = services.BuildServiceProvider();

var downloader = serviceProvider.GetRequiredService<IDownloader>();
var downloadResult = await downloader.DownloadAsync("downloads");
downloadResult.StartupFileNames = ["startup_china-maya.bin", "startup_maya-egypt.bin", "startup_egypt-vikings.bin"];
var webAppOutputDir = @"D:\IngweLand\Projects\forge-of-games\src\WebApp\resources\data\hoh";
var functionsAppOutputDir = @"D:\IngweLand\Projects\forge-of-games\src\Functions\resources\data\hoh";
var localizationParser = serviceProvider.GetRequiredService<LocalizationParser>();
localizationParser.Parse(downloadResult.Directory,[webAppOutputDir, functionsAppOutputDir]);
var gameDesignDataParser = serviceProvider.GetRequiredService<GameDesignDataParser>();
gameDesignDataParser.Parse(downloadResult, [webAppOutputDir, functionsAppOutputDir]);

Console.Out.WriteLine("DONE");

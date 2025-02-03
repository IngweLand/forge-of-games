using System.Net;
using Ingweland.Fog.HohProtoParser;
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

services.Configure<HohCredentials>(
    configuration.GetSection(HohCredentials.CONFIGURATION_PROPERTY_NAME));

services.AddLogging(builder =>
{
    builder.ClearProviders();
    builder.AddSerilog();
});

services.AddHttpClient<IAuthenticationService, AuthenticationService>()
    .AddStandardResilienceHandler(options =>
    {
        options.Retry.BackoffType = DelayBackoffType.Exponential;
        options.Retry.MaxRetryAttempts = 3;
    });

services.AddHttpClient<IDownloader, Downloader>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip,
    })
    .AddStandardResilienceHandler(options =>
    {
        options.Retry.BackoffType = DelayBackoffType.Exponential;
        options.Retry.MaxRetryAttempts = 3;
    });

services.AddSharedServices();
services.AddAutoMapper(typeof(Program).Assembly);
services.AddTransient<LocalizationParser>();
services.AddTransient<GameDesignDataParser>();

var serviceProvider = services.BuildServiceProvider();

var downloader = serviceProvider.GetRequiredService<IDownloader>();
var downloadResult = await downloader.DownloadAsync("downloads");

var outputDir = @"D:\IngweLand\Projects\forge-of-games\src\WebApp\resources\data\hoh";
var localizationParser = serviceProvider.GetRequiredService<LocalizationParser>();
localizationParser.Parse(downloadResult.Directory,outputDir);
var gameDesignDataParser = serviceProvider.GetRequiredService<GameDesignDataParser>();
gameDesignDataParser.Parse(Path.Combine(downloadResult.Directory, downloadResult.GamedesignFileName), outputDir);

Console.Out.WriteLine("DONE");

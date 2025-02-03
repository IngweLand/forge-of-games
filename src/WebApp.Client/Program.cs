using System.Diagnostics;
using Ingweland.Fog.Application.Client.Web;
using Ingweland.Fog.WebApp.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Globalization;
using Blazored.LocalStorage;
using Ingweland.Fog.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSharedServices();
builder.Services.AddLocalization();
builder.Services.AddMudServices();
builder.Services.AddWebAppApplicationServices();
builder.Services.AddWebAppClientServices(builder.HostEnvironment.BaseAddress);
builder.Services.AddBlazoredLocalStorage();
builder.AddWebAppClientSettings();

if (builder.HostEnvironment.IsProduction())
{
    builder.Logging.SetMinimumLevel(LogLevel.Warning);
}

var app = builder.Build();

var localeService = app.Services.GetRequiredService<IClientLocaleService>();
var localeInfo = await localeService.GetCurrentLocaleAsync();
var culture = CultureInfo.GetCultureInfo(localeInfo.Code);
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
await app.RunAsync();

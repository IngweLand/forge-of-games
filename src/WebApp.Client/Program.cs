using Ingweland.Fog.Application.Client.Web;
using Ingweland.Fog.WebApp.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Globalization;
using Blazored.LocalStorage;
using Ingweland.Fog.Application.Core.Interfaces;
using Ingweland.Fog.Shared;
using Ingweland.Fog.SyncfusionLicensing;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
FogSyncfusionLicenseProvider.Register();
builder.Services.AddSharedServices();
builder.Services.AddLocalization();
builder.Services.AddMudServices();
builder.Services.AddWebAppApplicationServices();
builder.Services.AddWebAppClientServices(builder.HostEnvironment.BaseAddress);
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSyncfusionBlazor();
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
var rep = app.Services.GetRequiredService<IHohGameLocalizationDataRepository>();
await rep.InitializeAsync();
await app.RunAsync();

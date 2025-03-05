using Ingweland.Fog.WebApp.Client.Models;

namespace Ingweland.Fog.WebApp.Client.Services.Abstractions;

public interface IClientLocaleService
{
    Task<LocaleInfo> GetCurrentLocaleAsync();
    Task SetLocaleAsync(LocaleInfo locale);
    IReadOnlyCollection<LocaleInfo> SupportedLocales { get; }
}
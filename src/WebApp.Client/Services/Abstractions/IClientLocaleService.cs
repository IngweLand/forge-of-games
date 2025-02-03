using Ingweland.Fog.WebApp.Client.Models;

public interface IClientLocaleService
{
    Task<LocaleInfo> GetCurrentLocaleAsync();
    Task SetLocaleAsync(LocaleInfo locale);
    IReadOnlyCollection<LocaleInfo> SupportedLocales { get; }
}

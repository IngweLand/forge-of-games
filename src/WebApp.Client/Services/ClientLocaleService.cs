using System.Globalization;
using Ingweland.Fog.Shared;
using Ingweland.Fog.Shared.Localization;
using Ingweland.Fog.WebApp.Client.Models;
using Microsoft.JSInterop;

public class ClientLocaleService : IClientLocaleService
{
    private LocaleInfo? _selectedLocale;
    private readonly IJSRuntime _jsRuntime;

    public ClientLocaleService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;

        var locales = new List<LocaleInfo>();
        foreach (var c in HohSupportedCultures.AllCultures)
        {
            var culture = CultureInfo.GetCultureInfo(c);
            locales.Add(new LocaleInfo(c, culture.NativeName.Split(' ')[0].ToUpper(culture)));
        }

        SupportedLocales = locales;
    }

    public IReadOnlyCollection<LocaleInfo> SupportedLocales { get; }

    public async Task<LocaleInfo> GetCurrentLocaleAsync()
    {
        if (_selectedLocale != null)
        {
            return _selectedLocale;
        }
        
        var savedLocaleCode = await _jsRuntime.InvokeAsync<string>("Fog.Webapp.LocaleManager.getLocale");
        if (string.IsNullOrWhiteSpace(savedLocaleCode))
        {
            var userLanguages = await _jsRuntime.InvokeAsync<string[]?>("Fog.Webapp.LocaleManager.getUserLanguages");
            if (userLanguages is {Length: > 0})
            {
               
                foreach (var userLanguage in userLanguages)
                {
                    _selectedLocale = SupportedLocales.FirstOrDefault(l => l.Code.StartsWith(userLanguage));
                }
            }
        }
        else
        {
            _selectedLocale = SupportedLocales.FirstOrDefault(l => l.Code.StartsWith(savedLocaleCode));
        }

        return _selectedLocale ??= SupportedLocales.First(li => li.Code == HohSupportedCultures.DefaultCulture);
    }

    public async Task SetLocaleAsync(LocaleInfo locale)
    {
        _selectedLocale = locale;
        var culture = CultureInfo.GetCultureInfo(locale.Code);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        await _jsRuntime.InvokeVoidAsync("Fog.Webapp.LocaleManager.setLocale", locale.Code);
    }
}

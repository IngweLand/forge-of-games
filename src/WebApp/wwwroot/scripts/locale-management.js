window.Fog = window.Fog || {};
window.Fog.Webapp = window.Fog.Webapp || {};
window.Fog.Webapp.LocaleManager = {
    LOCALE_KEY: '.AspNetCore.Culture',

    getUserLanguages: function () {
        return navigator.languages;
    },
    
    getLocale: function () {
        let locale;

        try {
            return localStorage.getItem(this.LOCALE_KEY);
        } catch (e) {
            console.warn('Error accessing localStorage:', e);
        }
    },

    setLocale: function (locale) {
        try {
            localStorage.setItem(this.LOCALE_KEY, locale);
        } catch (e) {
            console.warn('Error accessing localStorage:', e);
            return;
        }

        document.cookie = `${this.LOCALE_KEY}=c=${locale}|uic=${locale}; path=/; max-age=31536000; SameSite=Strict; Secure`;

        window.dispatchEvent(new CustomEvent('localeChanged', { detail: locale }));
        
        location.reload();
    },

    init: function () {
        const currentLocale = this.getLocale();
        if (currentLocale !== localStorage.getItem(this.LOCALE_KEY)) {
            this.setLocale(currentLocale);
        }
    }
};


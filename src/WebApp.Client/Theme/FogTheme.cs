using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Theme;

public class FogTheme
{
    public static MudTheme Theme = new()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#316896",
        },
        Typography = new Typography()
        {
            Default = new Default()
            {
                FontFamily = ["Noto Sans", "Segoe UI", "Helvetica Neue", "Arial", "sans-serif"],
            },
        }
    };
}

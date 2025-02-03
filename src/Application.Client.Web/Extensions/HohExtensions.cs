using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Client.Web.Extensions;

public static class HohExtensions
{
    private static readonly IList<string> AgeCssColors = new List<string>
        {"#BF6060", "#E8952E", "#5DC298", "#5A98BD", "#686DC4"};

    public static string ToCssColor(this Age? age)
    {
        return age != null? GetCssColor(age.Index):GetCssColor(0);
    }

    public static string ToCssColor(this AgeDto? age)
    {
        return age != null? GetCssColor(age.Index):GetCssColor(0);
    }

    private static string GetCssColor(int index)
    {
        var i = index - 2;
        if (i < 0)
        {
            i = 0;
        }

        return AgeCssColors[i % AgeCssColors.Count];
    }
}

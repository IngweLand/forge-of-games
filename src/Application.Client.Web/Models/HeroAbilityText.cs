using System.Text;
using System.Text.RegularExpressions;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Client.Web.Models;

public class HeroAbilityText
{
    private const string TITLE_PATTERN = @"<style=ability_label>([^<]+)</style>(.*)";
    private const string DESCRIPTION_ITEM_OPEN_TAG = "<style=ability_desc>";
    private const string STYLE_CLOSE_TAG = "</style>";
    private readonly string? _descriptionTemplate;

    public HeroAbilityText(string template)
    {
        var match = Regex.Match(template, TITLE_PATTERN);
        if (match is {Success: true, Groups.Count: 3})
        {
            Title = string.Join(" | ", match.Groups[1].Value.Split('|'));
            _descriptionTemplate = SetDescriptionStyles(match.Groups[2].Value);
        }
    }

    public string Title { get; } = string.Empty;

    public string GetDescription(IReadOnlyCollection<BattleAbilityDescriptionItem> descriptionItems)
    {
        if (_descriptionTemplate == null)
        {
            return string.Empty;
        }

        return descriptionItems.Aggregate(_descriptionTemplate,
            (current, descriptionItem) =>
                current.Replace($"{{{descriptionItem.Id}}}", descriptionItem.Value.ToFormatedString()));
    }

    private string SetDescriptionStyles(string input)
    {
        var results = new List<string>();
        var pos = 0;
        while (true)
        {
            var start = input.IndexOf(DESCRIPTION_ITEM_OPEN_TAG, pos, StringComparison.Ordinal);
            if (start == -1)
            {
                break;
            }

            var nextStart = input.IndexOf(DESCRIPTION_ITEM_OPEN_TAG, start + 1, StringComparison.Ordinal);

            var searchEndIndex = nextStart == -1 ? input.Length : nextStart;
            results.Add(input.Substring(start, searchEndIndex - STYLE_CLOSE_TAG.Length - start));
            pos = searchEndIndex;
        }

        var sb = new StringBuilder();
        sb.Append(@"<ul class='hero-ability-description-items'>");
        foreach (var result in results)
        {
            sb.Append(result.Replace(DESCRIPTION_ITEM_OPEN_TAG, @"<li>")
                .Replace(@"<style=basestat>", @"<span class='hero-ability-basestat'>")
                .Replace(@"<style=ability_link>", @"<span class='hero-ability-link'>")
                .Replace(@"</style>", @"</span>"));
            sb.Append("</li>");
        }

        sb.Append("</ul>");

        return sb.ToString();
    }
}

using System.Text.RegularExpressions;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Client.Web.Models;

public class RelicAbilityText
{
    private readonly string? _descriptionTemplate;
    private const string TITLE_PATTERN = @"<style=ability_label>([^<]+)</style>(.*)";

    public RelicAbilityText(string template)
    {
        var match = Regex.Match(template, TITLE_PATTERN);
        if (match is {Success: true, Groups.Count: 3})
        {
            Title = string.Join(" | ", match.Groups[1].Value.Split('|'));
            _descriptionTemplate = SetBaseStatStyle(match.Groups[2].Value);
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

    private string SetBaseStatStyle(string input)
    {
        return input.Replace(@"<style=basestat>", @"<span class='hero-ability-basestat'>")
            .Replace(@"</style>", @"</span>");
    }
}

using System.Globalization;
using System.Text.RegularExpressions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Localization;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Functions.Services;

public interface IHeroAttributeFeaturesParser
{
    Task RunAsync();
}

public class HeroAttributeFeaturesParser(
    IFogDbContext context,
    IUnitService unitService,
    DatabaseWarmUpService databaseWarmUpService) : IHeroAttributeFeaturesParser
{
    private const string TAG_PATTERN = @"<style=ability_label>([^<]+)</style>";
    private const string ATTRIBUTE_PATTERN = @"<style=ability_link>([^<]+)</style>";

    public async Task RunAsync()
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();

        var existing = (await context.HeroAbilityFeatures.ToListAsync()).ToDictionary(x => (x.HeroId, x.Locale));
        var newItems = new Dictionary<(string HeroId, string Locale), HeroAbilityFeaturesEntity>();
        foreach (var locale in HohSupportedCultures.AllCultures)
        {
            var culture = CultureInfo.GetCultureInfo(locale);
            CultureInfo.CurrentCulture = culture;
            var newHeroIds = (await unitService.GetHeroesBasicDataAsync()).Select(x => x.Id);
            foreach (var heroId in newHeroIds)
            {
                var hero = await unitService.GetHeroAsync(heroId);
                var tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var attributes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var lvl in hero!.Ability.Levels.Where(x => !string.IsNullOrWhiteSpace(x.Description)))
                {
                    var tagMatches = Regex.Matches(lvl.Description!, TAG_PATTERN);
                    foreach (Match tagMatch in tagMatches)
                    {
                        tags.UnionWith(tagMatch.Groups[1].Value.Split('|', '・').Select(x => x.Trim()));
                    }

                    var attributeMatches = Regex.Matches(lvl.Description!, ATTRIBUTE_PATTERN);
                    foreach (Match attributeMatch in attributeMatches)
                    {
                        attributes.Add(attributeMatch.Groups[1].Value.Trim());
                    }
                }

                newItems.Add((heroId, locale), new HeroAbilityFeaturesEntity
                {
                    HeroId = heroId,
                    Tags = tags,
                    Attributes = attributes,
                    Locale = locale,
                });
            }
        }

        foreach (var kvp in newItems)
        {
            if (existing.TryGetValue(kvp.Key, out var existingItem))
            {
                existingItem.Tags = kvp.Value.Tags;
                existingItem.Attributes = kvp.Value.Attributes;
            }
            else
            {
                context.HeroAbilityFeatures.Add(kvp.Value);
            }
        }

        await context.SaveChangesAsync();
    }
}

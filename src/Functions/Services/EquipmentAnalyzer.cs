using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IEquipmentAnalyzer
{
    Task Analyze();
}

public class EquipmentAnalyzer(IFogDbContext context, ILogger<IEquipmentAnalyzer> logger) : IEquipmentAnalyzer
{
    private static readonly IReadOnlyCollection<HeroLevelRange> LevelRanges =
    [
        new(0, 19), new(20, 39), new(40, 59), new(60, 79), new(80, 99), new(100, 119), new(120, 139),
        new(140, int.MaxValue),
    ];

    public async Task Analyze()
    {
        await context.EquipmentInsights.ExecuteDeleteAsync();
        
        var mainAttributes = new List<EquipmentStatItem>();
        var i = 0;
        await foreach (var squad in context.ProfileSquadDataItems.AsAsyncEnumerable())
        {
            var hero = squad.Hero;

            foreach (var equipmentItem in hero.Equipment)
            {
                mainAttributes.Add(new EquipmentStatItem
                {
                    UnitId = hero.UnitId,
                    LevelRange = GetLevelRange(hero.Level),
                    SlotType = equipmentItem.EquipmentSlotType,
                    Rarity = equipmentItem.EquipmentRarity,
                    Set = equipmentItem.EquipmentSet,
                    MainAttribute = equipmentItem.MainAttribute.StatAttribute,
                    SubAttributeLevel4 = equipmentItem.SubAttributes.FirstOrDefault(x => x.UnlockedAtLevel == 4)
                        ?.StatAttribute,
                    SubAttributeLevel8 = equipmentItem.SubAttributes.FirstOrDefault(x => x.UnlockedAtLevel == 8)
                        ?.StatAttribute,
                    SubAttributeLevel12 = equipmentItem.SubAttributes.FirstOrDefault(x => x.UnlockedAtLevel == 12)
                        ?.StatAttribute,
                    SubAttributeLevel16 = equipmentItem.SubAttributes.FirstOrDefault(x => x.UnlockedAtLevel == 16)
                        ?.StatAttribute,
                });
            }
            i++;
            if (i % 1000 == 0)
            {
                logger.LogInformation("Processed {Count} squads", i);
            }
        }

        logger.LogInformation("Finished getting squads");
        
        var mainGroupsBySlot = mainAttributes.GroupBy(x => (x.UnitId, x.SlotType));

        foreach (var g in mainGroupsBySlot)
        {
            logger.LogInformation("Processing insights for unit {UnitId} slot {SlotType}", g.Key.UnitId, g.Key.SlotType);
            
            var byLevelRange = g.GroupBy(x => x.LevelRange);
            foreach (var lrg in byLevelRange)
            {
                context.EquipmentInsights.Add(new EquipmentInsightsEntity
                {
                    UnitId = g.Key.UnitId,
                    EquipmentSlotType = g.Key.SlotType,
                    FromLevel = lrg.Key.From!.Value,
                    ToLevel = lrg.Key.To!.Value,
                    EquipmentSets = GetTopItems(lrg, x => x.Set),
                    MainAttributes = GetTopItems(lrg, x => x.MainAttribute),
                    SubAttributesLevel4 = GetTopItems2(lrg, x => x.SubAttributeLevel4),
                    SubAttributesLevel8 = GetTopItems2(lrg, x => x.SubAttributeLevel8),
                    SubAttributesLevel12 = GetTopItems2(lrg, x => x.SubAttributeLevel12),
                    SubAttributesLevel16 = GetTopItems2(lrg, x => x.SubAttributeLevel16),
                });
            }
        }

        logger.LogInformation("Saving equipment insights to database");
        await context.SaveChangesAsync();
        logger.LogInformation("Equipment insights saved successfully");
    }

    private List<T> GetTopItems<T>(IEnumerable<EquipmentStatItem> items, Func<EquipmentStatItem, T> selector)
    {
        return items.GroupBy(selector)
            .OrderBy(x => x.Count())
            .Take(3)
            .Select(x => x.Key)
            .ToList();
    }

    private List<StatAttribute> GetTopItems2(IEnumerable<EquipmentStatItem> items,
        Func<EquipmentStatItem, StatAttribute?> selector)
    {
        return items
            .Where(x => selector(x) != null)
            .GroupBy(selector)
            .OrderBy(x => x.Count())
            .Take(3)
            .Select(x => x.Key!.Value)
            .ToList();
    }

    private HeroLevelRange GetLevelRange(int level)
    {
        return LevelRanges.First(x => x.From <= level && level <= x.To);
    }

    private record EquipmentStatItem
    {
        public required HeroLevelRange LevelRange { get; init; }

        public required StatAttribute MainAttribute { get; init; }
        public required EquipmentRarity Rarity { get; init; }
        public required EquipmentSet Set { get; init; }
        public required EquipmentSlotType SlotType { get; init; }
        public StatAttribute? SubAttributeLevel12 { get; set; }
        public StatAttribute? SubAttributeLevel16 { get; set; }
        public StatAttribute? SubAttributeLevel4 { get; set; }
        public StatAttribute? SubAttributeLevel8 { get; set; }
        public required string UnitId { get; init; }
    }
}

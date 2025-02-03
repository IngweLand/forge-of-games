using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Extensions;

public static class BuildingGroupExtensions
{
    public static string ToStringRepresentation(this BuildingGroup group)
    {
        return group switch
        {
            BuildingGroup.Artisan => "artisan",
            BuildingGroup.AverageGoldMine => "averageGoldMine",
            BuildingGroup.AverageHome => "averageHome",
            BuildingGroup.AveragePapyrusField => "averagePapyrusField",
            BuildingGroup.AverageRiceFarm => "averageRiceFarm",
            BuildingGroup.Carpenter => "carpenter",
            BuildingGroup.CavalryBarracks => "cavalryBarracks",
            BuildingGroup.Channel => "channel",
            BuildingGroup.CityHall => "cityHall",
            BuildingGroup.ClayProcessor => "clayProcessor",
            BuildingGroup.CollectableAmphitheatre => "collectableAmphitheatre",
            BuildingGroup.CollectableArchitectsStudioV2 => "collectableArchitectsStudioV2",
            BuildingGroup.CollectableMinoanWatchtowerV2 => "collectableMinoanWatchtowerV2",
            BuildingGroup.CollectableSchoolV2 => "collectableSchoolV2",
            BuildingGroup.CompactCulture => "compactCulture",
            BuildingGroup.DomesticFarm => "domesticFarm",
            BuildingGroup.EvolvingFountainOfYouth => "evolvingFountainOfYouth",
            BuildingGroup.EvolvingGarden => "evolvingGarden",
            BuildingGroup.Fountain => "fountain",
            BuildingGroup.Goldsmith => "goldsmith",
            BuildingGroup.HeavyInfantryBarracks => "heavyInfantryBarracks",
            BuildingGroup.HeroAcademy => "heroAcademy",
            BuildingGroup.InfantryBarracks => "infantryBarracks",
            BuildingGroup.IrrigationStation => "irrigationStation",
            BuildingGroup.KaolinQuarry => "kaolinQuarry",
            BuildingGroup.LargeCulture => "largeCulture",
            BuildingGroup.LittleCulture => "littleCulture",
            BuildingGroup.ModerateCulture => "moderateCulture",
            BuildingGroup.MothGlade => "mothGlade",
            BuildingGroup.Oasis => "oasis",
            BuildingGroup.PapyrusPress => "papyrusPress",
            BuildingGroup.PorcelainWorkshop => "porcelainWorkshop",
            BuildingGroup.PremiumCulture => "premiumCulture",
            BuildingGroup.PremiumFarm => "premiumFarm",
            BuildingGroup.PremiumGoldMine => "premiumGoldMine",
            BuildingGroup.PremiumHome => "premiumHome",
            BuildingGroup.PremiumPapyrusField => "premiumPapyrusField",
            BuildingGroup.PremiumRiceFarm => "premiumRiceFarm",
            BuildingGroup.RangedBarracks => "rangedBarracks",
            BuildingGroup.RuralFarm => "ruralFarm",
            BuildingGroup.Scribe => "scribe",
            BuildingGroup.SiegeBarracks => "siegeBarracks",
            BuildingGroup.SilkWorkshop => "silkWorkshop",
            BuildingGroup.SmallHome => "smallHome",
            BuildingGroup.SmallWell => "smallWell",
            BuildingGroup.SpiceMerchant => "spiceMerchant",
            BuildingGroup.StoneMason => "stoneMason",
            BuildingGroup.Tailor => "tailor",
            BuildingGroup.ThreadProcessor => "threadProcessor",
            BuildingGroup.WaterPump => "waterPump",
            BuildingGroup.AverageBeehive => "averageBeehive",
            BuildingGroup.ExpeditionPier => "expeditionPier",
            BuildingGroup.PremiumSailorPort => "premiumSailorPort",
            BuildingGroup.WorkerHome => "workerHome",
            BuildingGroup.SailorHome => "sailorHome",
            BuildingGroup.AveragePier => "averagePier",
            BuildingGroup.PremiumPier => "premiumPier",
            BuildingGroup.Tavern => "tavern",
            BuildingGroup.HomeRunestone => "homeRunestone",
            BuildingGroup.BeehiveRunestone => "beehiveRunestone",
            BuildingGroup.TavernRunestone => "tavernRunestone",
            BuildingGroup.SailorPort => "sailorPort",
            _ => string.Empty,
        };
    }
}

using System.Text.Json;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Shared;
using Ingweland.Fog.Shared.Localization;
using Newtonsoft.Json;

var searchKeys = new List<string>()
{
    "Base.CampaignPanel.Campaign",
    "Base.QuestlinesPanel.EncounterOfRegion",
    "Base.HeroPanel.Ability",
    "Base.HeroPanel.AwakeningSuccessful",
    "Base.BuildingTypes.barracks_Name",
    "Base.ShopPopup.Heroes",
    "Base.BattleStatsPanel.Hero",
    "Base.HeroPanel.LVL",
    "Base.HeroFilterPopover.Level",
    "Base.HeroFilterPopover.AbilityLevel",
    "Base.Buildings.default_Name",
    "Base.TreasureHunt.Header",
    "Base.UnlockableFeatures.Wonders",
    "Base.WorldWondersInventionInformationPopup.WonderRewardLabel",
    "Base.Generic.MainCity",
    "Base.Generic.Resource",
    "Base.WorldWondersDetailPanel.WonderHeader",
    "Base.BattleStatsPanel.Hero",
    "Base.ResearchTreePanel.Header",
    "Base.BuildingContextPanel.Customization",
    "Base.BuildingContextPanel.ProductionLabel",
    "Base.Generic.Cancel",
    "Base.BuildingContextPanel.InfoTab",
    "Base.CampaignPanel.Battle",
};

var result = new Dictionary<string, List<Translations>>();
foreach (var localeCode in HohSupportedCultures.AllCultures)
{
    var filePath = GetInputFilePath(localeCode);

    using var localizationFile = File.OpenRead(filePath);
    var data = LocaResponseContainer.Parser.ParseFrom(localizationFile).Data.Entries
        .ToDictionary(entry => entry.Key, entry => entry.Values);

    var translations = searchKeys.Select(searchKey => new Translations() {Key = searchKey, Strings = data[searchKey]})
        .ToList();

    result.Add(localeCode, translations);
}
File.WriteAllText("translations.json", JsonConvert.SerializeObject(result, Formatting.Indented));

static string GetInputFilePath(string localeCode)
{
    var dir = @"D:\IngweLand\Projects\forge-of-games\resources\hoh\data\";
    var fileName = $"loca_{localeCode}.bin";
    return $"{dir}{fileName}";
}

internal class Translations
{
    public string Key { get; set; }
    public IList<string> Strings { get; set; }
}

using System.Web;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class BattleSearchRequestFactory : IBattleSearchRequestFactory
{
    private const string BattleTypeKey = "battleType";
    private const string CampaignRegionKey = "campaignRegion";
    private const string CampaignRegionEncounterKey = "campaignRegionEncounter";
    private const string DifficultyKey = "difficulty";
    private const string TreasureHuntDifficultyKey = "treasureHuntDifficulty";
    private const string TreasureHuntEncounterKey = "treasureHuntEncounter";
    private const string TreasureHuntStageKey = "treasureHuntStage";

    private static readonly BattleSearchRequest DefaultInfo = new();

    public BattleSearchRequest Create(string uri)
    {
        if (string.IsNullOrWhiteSpace(uri))
        {
            return new BattleSearchRequest();
        }

        var query = HttpUtility.ParseQueryString(new Uri(uri).Query);

        if (!query.HasKeys())
        {
            return new BattleSearchRequest();
        }

        var battleTypeValue = query[BattleTypeKey];
        if (string.IsNullOrWhiteSpace(battleTypeValue) ||
            !Enum.TryParse<BattleType>(battleTypeValue, out var battleType))
        {
            battleType = DefaultInfo.BattleType;
        }

        var campaignRegionEncounterValue = query[CampaignRegionEncounterKey];
        if (string.IsNullOrWhiteSpace(campaignRegionEncounterValue) ||
            !int.TryParse(campaignRegionEncounterValue, out var campaignRegionEncounter))
        {
            campaignRegionEncounter = DefaultInfo.CampaignRegionEncounter;
        }

        var difficultyValue = query[DifficultyKey];
        if (string.IsNullOrWhiteSpace(difficultyValue) ||
            !Enum.TryParse<Difficulty>(difficultyValue, out var difficulty))
        {
            difficulty = DefaultInfo.Difficulty;
        }

        var campaignRegionValue = query[CampaignRegionKey];
        if (string.IsNullOrWhiteSpace(campaignRegionValue) ||
            !Enum.TryParse<RegionId>(campaignRegionValue, out var regionId))
        {
            regionId = DefaultInfo.CampaignRegion;
        }

        int.TryParse(query[TreasureHuntDifficultyKey], out var treasureHuntDifficulty);
        int.TryParse(query[TreasureHuntEncounterKey], out var treasureHuntEncounter);
        int.TryParse(query[TreasureHuntStageKey], out var treasureHuntStage);

        return new BattleSearchRequest
        {
            BattleType = battleType,
            CampaignRegion = regionId,
            CampaignRegionEncounter = campaignRegionEncounter,
            Difficulty = difficulty,
            TreasureHuntDifficulty = treasureHuntDifficulty,
            TreasureHuntEncounter = treasureHuntEncounter,
            TreasureHuntStage = treasureHuntStage,
        };
    }

    public Dictionary<string, object?> CreateQueryParams(BattleSearchRequest request)
    {
        return new Dictionary<string, object?>
        {
            [BattleTypeKey] = request.BattleType.ToString(),
            [CampaignRegionKey] = request.CampaignRegion.ToString(),
            [CampaignRegionEncounterKey] = request.CampaignRegionEncounter,
            [DifficultyKey] = request.Difficulty.ToString(),
            [TreasureHuntDifficultyKey] = request.TreasureHuntDifficulty,
            [TreasureHuntEncounterKey] = request.TreasureHuntEncounter,
            [TreasureHuntStageKey] = request.TreasureHuntStage,
        };
    }
}

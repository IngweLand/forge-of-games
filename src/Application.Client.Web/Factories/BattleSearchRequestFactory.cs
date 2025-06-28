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
    private const string HistoricBattleRegionKey = "historicBattleRegion";
    private const string HistoricBattleEncounterKey = "historicBattleEncounter";
    private const string UnitIdKey = "unitId";

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

        var historicBattleRegionValue = query[HistoricBattleRegionKey];
        if (string.IsNullOrWhiteSpace(historicBattleRegionValue) ||
            !Enum.TryParse<RegionId>(historicBattleRegionValue, out var historicBattleRegionId))
        {
            historicBattleRegionId = DefaultInfo.HistoricBattleRegion;
        }

        int.TryParse(query[HistoricBattleEncounterKey], out var historicBattleEncounter);

        return new BattleSearchRequest
        {
            BattleType = battleType,
            CampaignRegion = regionId,
            CampaignRegionEncounter = campaignRegionEncounter,
            Difficulty = difficulty,
            TreasureHuntDifficulty = treasureHuntDifficulty,
            TreasureHuntEncounter = treasureHuntEncounter,
            TreasureHuntStage = treasureHuntStage,
            HistoricBattleRegion = historicBattleRegionId,
            HistoricBattleEncounter = historicBattleEncounter,
            UnitIds = query.GetValues(UnitIdKey) ?? [],
        };
    }

    public IReadOnlyDictionary<string, object?> CreateQueryParams(BattleSearchRequest request)
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
            [HistoricBattleRegionKey] = request.HistoricBattleRegion.ToString(),
            [HistoricBattleEncounterKey] = request.HistoricBattleEncounter,
            [UnitIdKey] = request.UnitIds,
        }.AsReadOnly();
    }

    public IReadOnlyDictionary<string, object?> CreateQueryParams(string battleDefinitionId, Difficulty difficulty,
        BattleType battleType, IEnumerable<string>? unitIds, TreasureHuntEncounterMapDto? treasureHuntEncounterMap)
    {
        var queryParams = new Dictionary<string, object?>();
        if (string.IsNullOrWhiteSpace(battleDefinitionId))
        {
            return queryParams;
        }

        queryParams.Add(BattleTypeKey, battleType.ToString());
        queryParams.Add(DifficultyKey, difficulty.ToString());
        const char delimiter = '_';
        var battleDefinitionIdParts = battleDefinitionId.Split(delimiter);
        if (battleType == BattleType.Campaign)
        {
            queryParams.Add(CampaignRegionKey, $"{battleDefinitionIdParts[0]}{delimiter}{battleDefinitionIdParts[1]}");
            queryParams.Add(CampaignRegionEncounterKey, battleDefinitionIdParts[2]);
        }

        if (battleType == BattleType.TreasureHunt)
        {
            var athDifficulty = int.Parse(battleDefinitionIdParts[1]);
            var athStage = int.Parse(battleDefinitionIdParts[2]);
            var athEncounter = int.Parse(battleDefinitionIdParts[3]);
            queryParams.Add(TreasureHuntDifficultyKey, athDifficulty);
            queryParams.Add(TreasureHuntStageKey, athStage);
            if (treasureHuntEncounterMap != null)
            {
                if (treasureHuntEncounterMap.BattleEncounterMap.TryGetValue((athDifficulty, athStage), out var map) &&
                    map.TryGetValue(athEncounter, out var index))
                {
                    queryParams.Add(TreasureHuntEncounterKey, index);
                }
            }
        }

        if (battleType == BattleType.HistoricBattle)
        {
            queryParams.Add(HistoricBattleRegionKey, battleDefinitionIdParts[0]);
            queryParams.Add(HistoricBattleEncounterKey, battleDefinitionIdParts[1]);
        }

        if (unitIds != null)
        {
            queryParams.Add(UnitIdKey, unitIds.ToHashSet().ToArray());
        }

        return queryParams.AsReadOnly();
    }
}

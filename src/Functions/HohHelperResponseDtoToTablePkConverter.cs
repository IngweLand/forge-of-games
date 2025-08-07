using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Functions;

public class HohHelperResponseDtoToTablePkConverter(
    IInGameDataParsingService inGameDataParsingService,
    InGameRawDataTablePartitionKeyProvider tablePartitionKeyProvider)
{
    public IEnumerable<(string CollectionCategoryId, string PartitionKey)> Get(HohHelperResponseDto inGameData,
        DateOnly date)
    {
        var worldId = UriUtils.GetSubdomain(inGameData.ResponseUrl);
        var path = UriUtils.GetPath(inGameData.ResponseUrl);

        switch (path)
        {
            case "game/ranking/player":
            {
                var playerRankingType = GetPlayerRankingType(inGameData.Base64ResponseData!);
                yield return (inGameData.CollectionCategoryIds.First(),
                    tablePartitionKeyProvider.PlayerRankings(worldId, date, playerRankingType));
                break;
            }

            case "game/ranking/alliance":
            {
                var allianceRankingType = GetAllianceRankingType(inGameData.Base64ResponseData!);
                yield return (inGameData.CollectionCategoryIds.First(),
                    tablePartitionKeyProvider.AllianceRankings(worldId, date, allianceRankingType));
                break;
            }

            case "game/pvp/get-ranking":
            {
                yield return (inGameData.CollectionCategoryIds.First(),
                    tablePartitionKeyProvider.PvpRankings(worldId, date));
                break;
            }

            case "game/wakeup":
            {
                foreach (var collectionCategoryId in inGameData.CollectionCategoryIds)
                {
                    switch (collectionCategoryId)
                    {
                        case "leaderboards":
                        {
                            yield return (collectionCategoryId,
                                tablePartitionKeyProvider.AthAllianceRankings(worldId, date));
                            break;
                        }

                        case "alliance":
                        {
                            yield return (collectionCategoryId, tablePartitionKeyProvider.Alliance(worldId, date));
                            break;
                        }
                    }
                }

                break;
            }

            case "game/pvp/get-battle-history":
            {
                yield return (inGameData.CollectionCategoryIds.First(),
                    tablePartitionKeyProvider.PvpBattles(worldId, date));
                break;
            }

            case "game/battle/hero/stats":
            {
                yield return (inGameData.CollectionCategoryIds.First(),
                    tablePartitionKeyProvider.BattleStats(worldId, date));
                break;
            }

            case "game/battle/hero/complete-wave":
            {
                yield return (inGameData.CollectionCategoryIds.First(),
                    tablePartitionKeyProvider.BattleCompleteWave(worldId, date));
                break;
            }

            case "game/battle/hero/start":
            {
                yield return (inGameData.CollectionCategoryIds.First(),
                    tablePartitionKeyProvider.BattleStart(worldId, date));
                break;
            }
        }
    }

    private PlayerRankingType GetPlayerRankingType(string base64ResponseData)
    {
        var ranks = inGameDataParsingService.ParsePlayerRanking(base64ResponseData);
        if (!Enum.TryParse(ranks.Type.ToString(), out PlayerRankingType playerRankingType))
        {
            throw new Exception(
                $"Cannot map {ranks.Type.ToString()} from {typeof(Inn.Models.Hoh.PlayerRankingType).FullName} to {
                    typeof(PlayerRankingType).FullName}");
        }

        return playerRankingType;
    }

    private AllianceRankingType GetAllianceRankingType(string base64ResponseData)
    {
        var ranks = inGameDataParsingService.ParseAllianceRankings(base64ResponseData!);
        if (!Enum.TryParse(ranks.Type.ToString(), out AllianceRankingType allianceRankingType))
        {
            throw new Exception(
                $"Cannot map {ranks.Type.ToString()} from {typeof(Inn.Models.Hoh.AllianceRankingType).FullName} to {
                    typeof(AllianceRankingType).FullName}");
        }

        return allianceRankingType;
    }
}

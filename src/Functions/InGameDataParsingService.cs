using Ingweland.Fog.Application.Server.Services.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions;

public class InGameDataParsingService(
    IDataParsingService dataParsingService,
    ILogger<InGameDataProcessingServiceBase> logger)
    : InGameDataProcessingServiceBase(logger), IInGameDataParsingService
{
    public PlayerRanks ParsePlayerRanking(string inputData)
    {
        var data = DecodeInternal(inputData);

        return dataParsingService.ParsePlayerRankings(data);
    }

    public IReadOnlyCollection<PvpRank> ParsePvpRanking(string inputData)
    {
        var data = DecodeInternal(inputData);

        return dataParsingService.ParsePvpRankings(data);
    }

    public IReadOnlyCollection<PvpBattle> ParsePvpBattles(string inputData)
    {
        var data = DecodeInternal(inputData);

        return dataParsingService.ParsePvpBattles(data);
    }

    public BattleSummary ParseBattleWaveResult(string inputData)
    {
        var data = DecodeInternal(inputData);

        return dataParsingService.ParseBattleWaveResult(data);
    }

    public BattleStats ParseBattleStats(string inputData)
    {
        var data = DecodeInternal(inputData);

        return dataParsingService.ParseBattleStats(data);
    }

    public AllianceRanks ParseAllianceRankings(string inputData)
    {
        var data = DecodeInternal(inputData);

        return dataParsingService.ParseAllianceRankings(data);
    }

    public Wakeup ParseWakeup(string inputData)
    {
        var data = DecodeInternal(inputData);

        return dataParsingService.ParseWakeup(data);
    }

    public byte[] Decode(string inputData)
    {
        return DecodeInternal(inputData);
    }
}

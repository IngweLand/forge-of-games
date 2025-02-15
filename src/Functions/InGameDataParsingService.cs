using Ingweland.Fog.Application.Server.Services.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
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

    public AllianceRanks ParseAllianceRankings(string inputData)
    {
        var data = DecodeInternal(inputData);

        return dataParsingService.ParseAllianceRankings(data);
    }

    public byte[] Decode(string inputData)
    {
        return DecodeInternal(inputData);
    }
}
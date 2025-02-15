using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.InnSdk.Hoh.Services;

public class DataParsingService(ILogger<DataParsingService> logger, IMapper mapper) : IDataParsingService
{
    public PlayerRanks ParsePlayerRankings(byte[] data)
    {
        PlayerRanksDTO ranksDto;
        try
        {
            var container = CommunicationDto.Parser.ParseFrom(data);
            ranksDto = container.PlayerRanks;
        }
        catch (Exception ex)
        {
            const string msg = "Failed to parse player ranking data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        return mapper.Map<PlayerRanks>(ranksDto);
    }
    
    public AllianceRanks ParseAllianceRankings(byte[] data)
    {
        AllianceRanksDTO ranksDto;
        try
        {
            var container = CommunicationDto.Parser.ParseFrom(data);
            ranksDto = container.AllianceRanks;
        }
        catch (Exception ex)
        {
            const string msg = "Failed to parse alliance ranking data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        return mapper.Map<AllianceRanks>(ranksDto);
    }
}
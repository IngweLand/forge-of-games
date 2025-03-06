using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities;
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

    public IReadOnlyCollection<PvpRank> ParsePvpRankings(byte[] data)
    {
        PvpGetRankingResponse ranksDto;
        try
        {
            var container = CommunicationDto.Parser.ParseFrom(data);
            ranksDto = container.PvpGetRankingResponse;
        }
        catch (Exception ex)
        {
            const string msg = "Failed to parse pvp ranking data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        return mapper.Map<IReadOnlyCollection<PvpRank>>(ranksDto.Rankings);
    }
    
    public Wakeup ParseWakeup(byte[] data)
    {
        CommunicationDto dto;
        try
        {
            dto = CommunicationDto.Parser.ParseFrom(data);
        }
        catch (Exception ex)
        {
            const string msg = "Failed to parse alliance member data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        return mapper.Map<Wakeup>(dto);
    }
}
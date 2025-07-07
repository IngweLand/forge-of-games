using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.City;
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

    public PlayerProfile ParsePlayerProfile(byte[] data)
    {
        PlayerProfileResponse dto;
        try
        {
            var container = CommunicationDto.Parser.ParseFrom(data);
            dto = container.PlayerProfileResponse;
        }
        catch (Exception ex)
        {
            const string msg = "Failed to parse player profile response data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        return mapper.Map<PlayerProfile>(dto);
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

    public BattleSummary ParseBattleStart(byte[] data)
    {
        HeroStartBattleResponse dto;
        try
        {
            var container = CommunicationDto.Parser.ParseFrom(data);
            dto = container.HeroStartBattleResponse;
        }
        catch (Exception ex)
        {
            const string msg = "Failed to parse hero start battle response data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        return mapper.Map<BattleSummary>(dto.Result);
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

    public BattleSummary ParseBattleWaveResult(byte[] data)
    {
        HeroFinishWaveResponse dto;
        try
        {
            var container = CommunicationDto.Parser.ParseFrom(data);
            dto = container.HeroFinishWaveResponse;
        }
        catch (Exception ex)
        {
            const string msg = "Failed to parse hero finish wave response data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        return mapper.Map<BattleSummary>(dto.Result);
    }

    public IReadOnlyCollection<PvpBattle> ParsePvpBattles(byte[] data)
    {
        PvpBattleHistoryResponseDto battlesDto;
        try
        {
            var container = SingleItemCommunicationDto.Parser.ParseFrom(data);
            battlesDto = container.PvpBattleHistoryResponse!;
        }
        catch (Exception ex)
        {
            const string msg = "Failed to parse pvp battles data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        var battlesOwner = GetPvpBattlesOwner(battlesDto.Battles);

        if (battlesOwner == -1)
        {
            logger.LogWarning("Could not identify the owner of pvp battles");
            return [];
        }

        return (from battleDto in battlesDto.Battles
            let player1IsOwner = battleDto.Player1.Id == battlesOwner
            let isVictory = battleDto.PointsDelta > 0
            let isPlayer1Winner = (player1IsOwner && isVictory) || (!player1IsOwner && !isVictory)
            let winnerDto = isPlayer1Winner ? battleDto.Player1 : battleDto.Player2
            let winnerUnitsDto = isPlayer1Winner ? battleDto.Player1Units : battleDto.Player2Units
            let loserDto = isPlayer1Winner ? battleDto.Player2 : battleDto.Player1
            let loserUnitsDto = isPlayer1Winner ? battleDto.Player2Units : battleDto.Player1Units
            let winner = mapper.Map<HohPlayer>(winnerDto)
            let winnerUnits = mapper.Map<IReadOnlyCollection<PvpUnit>>(winnerUnitsDto)
            let loser = mapper.Map<HohPlayer>(loserDto)
            let loserUnits = mapper.Map<IReadOnlyCollection<PvpUnit>>(loserUnitsDto)
            select new PvpBattle
            {
                Id = battleDto.Id.ToByteArray(),
                Winner = winner,
                Loser = loser,
                WinnerUnits = winnerUnits,
                LoserUnits = loserUnits,
                PerformedAt = battleDto.PerformedAt.ToDateTime(),
            }).ToList();
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

    public BattleStats ParseBattleStats(byte[] data)
    {
        HeroBattleStatsResponse dto;
        try
        {
            var container = CommunicationDto.Parser.ParseFrom(data);
            dto = container.HeroBattleStatsResponse;
        }
        catch (Exception ex)
        {
            const string msg = "Failed to parse hero battle stats response data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        return mapper.Map<BattleStats>(dto);
    }

    public OtherCity ParseOtherCity(byte[] data)
    {
        OtherCityDTO dto;
        try
        {
            var container = CommunicationDto.Parser.ParseFrom(data);
            dto = container.OtherCity;
        }
        catch (Exception ex)
        {
            const string msg = "Failed to parse other city data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        return mapper.Map<OtherCity>(dto);
    }

    private int GetPvpBattlesOwner(IList<PvpBattleDto> battles)
    {
        if (battles.Count == 0)
        {
            return -1;
        }

        var grouped = battles
            .SelectMany(b => new[] {b.Player1.Id, b.Player2.Id})
            .GroupBy(n => n)
            .ToDictionary(g => g.Key, g => g.Count());

        var max = grouped.MaxBy(kvp => kvp.Value);

        if (grouped.Count(kvp => kvp.Value == max.Value) > 1)
        {
            return -1;
        }

        return max.Key;
    }
}

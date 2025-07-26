using FluentResults;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;

namespace Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

public interface IDataParsingService
{
    AllianceRanks ParseAllianceRankings(byte[] data);
    StartupDto ParseStartupData(byte[] data);
    BattleSummary ParseBattleStart(byte[] data);
    BattleSummary ParseBattleWaveResult(byte[] data);
    IReadOnlyCollection<PvpBattle> ParsePvpBattles(byte[] data);
    IReadOnlyCollection<PvpRank> ParsePvpRankings(byte[] data);
    PlayerRanks ParsePlayerRankings(byte[] data);
    Result<PlayerProfile> ParsePlayerProfile(byte[] data);
    Wakeup ParseWakeup(byte[] data);
    BattleStats ParseBattleStats(byte[] data);
    OtherCity ParseOtherCity(byte[] data);
}

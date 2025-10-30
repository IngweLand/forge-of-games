using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Interfaces;

public interface IFogDbContext
{
    DbSet<AllianceRanking> AllianceRankings { get; set; }
    DbSet<Alliance> Alliances { get; set; }
    DbSet<BattleSummaryEntity> Battles { get; set; }
    DbSet<BattleStatsEntity> BattleStats { get; set; }
    DbSet<BattleTimelineEntity> BattleTimelines { get; set; }
    DbSet<BattleUnitEntity> BattleUnits { get; set; }
    DbSet<EquipmentInsightsEntity> EquipmentInsights { get; set; }
    DbSet<InGameEventEntity> InGameEvents { get; set; }
    DbSet<PlayerCitySnapshot> PlayerCitySnapshots { get; set; }
    DbSet<PlayerRanking> PlayerRankings { get; set; }
    DbSet<Player> Players { get; set; }
    DbSet<ProfileSquadDataEntity> ProfileSquadDataItems { get; set; }
    DbSet<ProfileSquadEntity> ProfileSquads { get; set; }
    DbSet<PvpBattle> PvpBattles { get; set; }
    DbSet<RelicInsightsEntity> RelicInsights { get; set; }
    DbSet<SharedSubmissionIdEntity> SharedSubmissionIds { get; set; }
    DbSet<TopHeroInsightsEntity> TopHeroInsights { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

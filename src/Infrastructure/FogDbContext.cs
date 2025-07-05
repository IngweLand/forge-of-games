using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Infrastructure.EntityConfigurations;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Infrastructure;

public class FogDbContext : DbContext, IFogDbContext
{
    public FogDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<PvpRanking> PvpRankings { get; set; }

    public DbSet<BattleUnitEntity> BattleUnits { get; set; }

    public DbSet<BattleSummaryEntity> Battles { get; set; }

    public DbSet<PlayerRanking> PlayerRankings { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Alliance> Alliances { get; set; }
    public DbSet<AllianceRanking> AllianceRankings { get; set; }
    public DbSet<PlayerAllianceNameHistoryEntry> PlayerAllianceNameHistory { get; set; }
    public DbSet<PvpBattle> PvpBattles { get; set; }
    public DbSet<BattleStatsEntity> BattleStats { get; set; }
    public DbSet<PlayerCitySnapshot> PlayerCitySnapshots { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new PlayerEntityTypeConfiguration());
        builder.ApplyConfiguration(new PlayerRankingEntityTypeConfiguration());
        builder.ApplyConfiguration(new PvpRankingEntityTypeConfiguration());
        builder.ApplyConfiguration(new PlayerNameHistoryEntryEntityTypeConfiguration());
        builder.ApplyConfiguration(new PlayerAgeHistoryEntryEntityTypeConfiguration());
        builder.ApplyConfiguration(new PlayerAllianceNameHistoryEntryEntityTypeConfiguration());
        builder.ApplyConfiguration(new AllianceEntityTypeConfiguration());
        builder.ApplyConfiguration(new AllianceRankingEntityTypeConfiguration());
        builder.ApplyConfiguration(new AllianceNameHistoryEntryEntityTypeConfiguration());
        builder.ApplyConfiguration(new PvpBattleEntityTypeConfiguration());
        builder.ApplyConfiguration(new BattleSummaryEntityTypeConfiguration());
        builder.ApplyConfiguration(new BattleUnitEntityTypeConfiguration());
        builder.ApplyConfiguration(new BattleStatsEntityTypeConfiguration());
        builder.ApplyConfiguration(new BattleSquadStatsEntityTypeConfiguration());
        builder.ApplyConfiguration(new UnitBattleStatsEntityTypeConfiguration());
        builder.ApplyConfiguration(new PlayerCitySnapshotEntityTypeConfiguration());
    }
}

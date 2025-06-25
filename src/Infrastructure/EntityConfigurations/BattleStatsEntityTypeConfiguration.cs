using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class BattleStatsEntityTypeConfiguration : IEntityTypeConfiguration<BattleStatsEntity>
{
    public void Configure(EntityTypeBuilder<BattleStatsEntity> builder)
    {
        builder.ToTable("battle_stats");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.InGameBattleId).IsRequired();

        builder.HasIndex(p => p.InGameBattleId).IsUnique();

        builder.HasMany(b => b.Squads)
            .WithOne()
            .HasForeignKey(s => s.BattleStatsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

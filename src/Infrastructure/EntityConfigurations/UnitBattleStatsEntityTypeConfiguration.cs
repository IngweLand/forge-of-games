using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class UnitBattleStatsEntityTypeConfiguration : IEntityTypeConfiguration<UnitBattleStatsEntity>
{
    public void Configure(EntityTypeBuilder<UnitBattleStatsEntity> builder)
    {
        builder.ToTable("unit_battle_stats");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.UnitId).IsRequired();
    }
}

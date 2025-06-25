using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class BattleSquadStatsEntityTypeConfiguration : IEntityTypeConfiguration<BattleSquadStatsEntity>
{
    public void Configure(EntityTypeBuilder<BattleSquadStatsEntity> builder)
    {
        builder.ToTable("battle_squad_stats");

        builder.HasKey(p => p.Id);
        builder.Property(s => s.Side).IsRequired();
    }
}
